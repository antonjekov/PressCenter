using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PressCenter.Services.Data;
using PressCenter.Web.ViewModels.News;

namespace FunctionGetNewPublications
{
    public class ClearPublications
    {
        
       [FunctionName("ClearPublications")]
        public static async Task RunAsync([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, INewsService newsService)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var news = newsService.GetAll<NewsViewModel>().Where(x=>x.Date>=DateTime.Today.AddDays(-1));
            foreach (var item in news)
            {
                await newsService.DeleteAsync(item.Id);
            }
        }
    }
}
