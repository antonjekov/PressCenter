using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PressCenter.Services.Data;

namespace AzurCronFunctions
{
    public class AzureCronJobs
    {
        private AzureDataContext azureDataContext;
        private INewsService newsService;

        public AzureCronJobs(AzureDataContext azureDataContext, INewsService newsService)
        {
            this.azureDataContext = azureDataContext;
            this.newsService = newsService;
        }

        [FunctionName("DeleteOldNews")]
        public async Task Run([TimerTrigger("0 0 3 * * *")]TimerInfo myTimer, ILogger log)
        {
            var newsToDelete = await azureDataContext.News.Where(x => x.Date <= DateTime.Today.AddDays(-30)).ToListAsync();
            foreach (var item in newsToDelete)
            {
                azureDataContext.News.Remove(item);
                await azureDataContext.SaveChangesAsync();
            }
            log.LogInformation($"Azure DeleteOldNews function executed at: {DateTime.Now}");
        }

        [FunctionName("SeedNews")]
        public async Task SeedNews([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer, ILogger log)
        {
            var newsToDelete = await azureDataContext.News.Where(x => x.Date <= DateTime.Today.AddDays(-30)).ToListAsync();
            foreach (var item in newsToDelete)
            {
                azureDataContext.News.Remove(item);
                await azureDataContext.SaveChangesAsync();
            }
            log.LogInformation($"Azure DeleteOldNews function executed at: {DateTime.Now}");
        }

        [FunctionName("GetAllNews")]
        public async Task<IActionResult> GetAllNews(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var allNews = await azureDataContext.News.FirstOrDefaultAsync();
            //var news = newsService.GetAll();
            return new OkObjectResult(allNews);
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
