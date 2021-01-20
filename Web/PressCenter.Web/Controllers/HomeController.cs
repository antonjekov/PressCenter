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
            var allNews = this.newsService.GetAll<NewsViewModel>(1, itemsPerPage);
            return this.View(allNews);
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
