namespace PressCenter.Web.Controllers
{
    using System.Diagnostics;

    using PressCenter.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using PressCenter.Services.Data;
    using PressCenter.Web.ViewModels.News;

    public class HomeController : BaseController
    {
        private readonly INewsService newsService;

        public HomeController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public IActionResult Index(int id = 1)
        {
            int itemsPerPage = 10;
            var allNewsThisPage = this.newsService.GetAll<NewsViewModel>(id, itemsPerPage);
            var allNewsCount = this.newsService.Count();
            var newsTodayCount = this.newsService.NewsTodayCount();
            var allNewsPagination = new NewsViewModelPagination()
            {
                ItemsCount = allNewsCount,
                ItemsPerPage = itemsPerPage,
                PageNumber = id,
                News = allNewsThisPage,
                ItemsTodayCount = newsTodayCount,
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
