namespace PressCenter.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;
    using PressCenter.Services.CronJobs;
    using PressCenter.Services.Data;
    using PressCenter.Web.ViewModels.News;
    using PressCenter.Web.ViewModels.TopNews;

    public class NewsController : BaseController
    {
        private INewsService newsService;
        private ISourceService sourceService;
        private ITopNewsService topNewsService;

        public NewsController(INewsService newsService, ITopNewsService topNewsService, ISourceService sourceService)
        {
            this.newsService = newsService;
            this.sourceService = sourceService;
            this.topNewsService = topNewsService;
        }

        public async Task<IActionResult> AddAsync()
        {
            var sources = this.sourceService.GetAll();
            foreach (var source in sources)
            {
                await new GetNewPublicationsJob(this.newsService).StartAsync(source);
            }

            return this.Json("ok");
        }

        public IActionResult Details(int id)
        {
            var news = this.newsService.GetById<NewsViewModel>(id);
            var topNews = this.topNewsService.GetAll<TopNewsViewModel>();
            var model = new NewsDetailsViewModel()
            {
                News = news,
                TopNews = topNews,
            };
            return news != null ? this.View(model) : this.RedirectToAction("Index", "Home");
        }
    }
}
