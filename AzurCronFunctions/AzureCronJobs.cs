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
        private readonly ApplicationDbContext azureDataContext;
        private readonly INewsService newsService;
        private readonly ISourceService sourceService;
        private readonly ITopNewsSourceService topNewsSourceService;
        private readonly IGetNewPublicationsJob newPublicationsJob;
        private readonly IGetNewTopNewsJob getNewTopNewsJob;

        public AzureCronJobs(ApplicationDbContext azureDataContext, INewsService newsService, ISourceService sourceService, ITopNewsSourceService topNewsSourceService, IGetNewPublicationsJob newPublicationsJob, IGetNewTopNewsJob getNewTopNewsJob)
        {
            this.azureDataContext = azureDataContext;
            this.newsService = newsService;
            this.sourceService = sourceService;
            this.topNewsSourceService = topNewsSourceService;
            this.newPublicationsJob = newPublicationsJob;
            this.getNewTopNewsJob = getNewTopNewsJob;
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

        // "*/30 * * * * *" - 30 seconds; "0 0 * * * *" - 1 hour; "0 */30 * * * *" - 30 minutes
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

        [FunctionName("SeedTopNews")]
        public async Task SeedTopNews([TimerTrigger("0 */45 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var sources = this.topNewsSourceService.GetAll();
            foreach (var item in sources)
            {
                await getNewTopNewsJob.StartAsync(item);
                log.LogInformation($"{item.Name} seed processed.");
            }
        }

        [FunctionName("GetAllNews")]
        public IActionResult GetAllNews(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var news = newsService.GetAll<NewsViewModel>();
            return new OkObjectResult(news);
        }

        [FunctionName("GetAllSources")]
        public IActionResult GetAllSources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var allSources = sourceService.GetAll();
            return new OkObjectResult(allSources);
        }
    }
}
