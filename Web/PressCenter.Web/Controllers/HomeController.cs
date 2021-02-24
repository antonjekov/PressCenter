namespace PressCenter.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using PressCenter.Services.Data;
    using PressCenter.Web.ViewModels;
    using PressCenter.Web.ViewModels.News;
    using PressCenter.Web.ViewModels.Sources;
    using PressCenter.Web.ViewModels.TopNews;

    public class HomeController : BaseController
    {
        private readonly INewsService newsService;
        private readonly ITopNewsService topNewsService;
        private readonly ISourceService sourceService;

        public HomeController(INewsService newsService, ITopNewsService topNewsService, ISourceService sourceService)
        {
            this.newsService = newsService;
            this.topNewsService = topNewsService;
            this.sourceService = sourceService;
        }

        public IActionResult Index(int id = 1)
        {
            int itemsPerPage = 5;
            IEnumerable<NewsViewModel> allNewsThisPage;
            int allNewsCount;
            int newsTodayCount;
            var selectedSourcesJson = this.Request.Cookies["sourceSelect"];

            if (selectedSourcesJson != null)
            {
                var selectedSources = JsonSerializer.Deserialize<List<int>>(selectedSourcesJson);
                allNewsThisPage = this.newsService.GetAll<NewsViewModel>(id, itemsPerPage, selectedSources);
                allNewsCount = this.newsService.Count(selectedSources);
                newsTodayCount = this.newsService.NewsTodayCount(selectedSources);
            }
            else
            {
                allNewsThisPage = this.newsService.GetAll<NewsViewModel>(id, itemsPerPage);
                allNewsCount = this.newsService.Count();
                newsTodayCount = this.newsService.NewsTodayCount();
            }

            var topNewsToday = this.topNewsService.GetAllFromToday<TopNewsViewModel>();
            var topNews = topNewsToday.GroupBy(x => x.SourceId).Select(g => g.ToList().Take(2)).SelectMany(x => x).ToList();
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
