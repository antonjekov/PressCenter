namespace PressCenter.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using PressCenter.Services.Data;
    using PressCenter.Web.ViewModels;
    using PressCenter.Web.ViewModels.News;
    using PressCenter.Web.ViewModels.TopNews;

    public class HomeController : BaseController
    {
        private readonly INewsService newsService;
        private readonly ITopNewsService topNewsService;

        public HomeController(INewsService newsService, ITopNewsService topNewsService)
        {
            this.newsService = newsService;
            this.topNewsService = topNewsService;
        }

        public IActionResult Index(int id = 1)
        {
            int itemsPerPage = 5;
            var allNewsThisPage = this.newsService.GetAll<NewsViewModel>(id, itemsPerPage);
            var allNewsCount = this.newsService.Count();
            var newsTodayCount = this.newsService.NewsTodayCount();
            var topNews = this.topNewsService.GetAllFromToday<TopNewsViewModel>();
            var allNewsPagination = new NewsViewModelPagination()
            {
                ItemsCount = allNewsCount,
                ItemsPerPage = itemsPerPage,
                PageNumber = id,
                News = allNewsThisPage,
                ItemsTodayCount = newsTodayCount,
                TopNews = topNews,
            };
            return this.View(allNewsPagination);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
