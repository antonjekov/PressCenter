using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PressCenter.Data;
using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using PressCenter.Services.CronJobs;
using PressCenter.Services.Data;
using PressCenter.Web.ViewModels.News;

namespace AzurCronFunctions
{
    public class AzureCronJobs
    {
        private ApplicationDbContext azureDataContext;
        private INewsService newsService;
        private ISourceService sourceService;
        private IGetNewPublicationsJob newPublicationsJob;

        public AzureCronJobs(ApplicationDbContext azureDataContext, INewsService newsService, ISourceService sourceService, IGetNewPublicationsJob newPublicationsJob)
        {
            this.azureDataContext = azureDataContext;
            this.newsService = newsService;
            this.sourceService = sourceService;
            this.newPublicationsJob = newPublicationsJob;
        }        

        [FunctionName("DeleteOldNews")]
        public async Task Run([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer, ILogger log)
        {
            var newsToDelete = await azureDataContext.News.Where(x => x.Date <= DateTime.Today.AddDays(-30)).ToListAsync();
            foreach (var item in newsToDelete)
            {
                azureDataContext.News.Remove(item);
                await azureDataContext.SaveChangesAsync();
            }
            log.LogInformation($"Azure DeleteOldNews function executed at: {DateTime.Now}");
        }

        // "*/30 * * * * *" - 30 secconds; "0 0 * * * *" - 1 hour;
        [FunctionName("SeedNews")]
        public async Task SeedNews([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var sources = this.sourceService.GetAll();
            foreach (var item in sources)
            {
                await newPublicationsJob.StartAsync(item);
                log.LogInformation($"{item.Name} seed processed.");
            }
        }

        [FunctionName("GetAllNews")]
        public async Task<IActionResult> GetAllNews(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var news = newsService.GetAll<NewsViewModel>();
            return new OkObjectResult(news);
        }

        [FunctionName("GetAllSources")]
        public async Task<IActionResult> GetAllSources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var allSources = await azureDataContext.Sources.FirstOrDefaultAsync();
            return new OkObjectResult(allSources);
        }
    }
}
