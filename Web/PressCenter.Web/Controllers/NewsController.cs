namespace PressCenter.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Xml;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;
    using PressCenter.Services.CronJobs;
    using PressCenter.Services.Data;
    using PressCenter.Services.RssAtom;
    using PressCenter.Web.ViewModels.News;
    using PressCenter.Web.ViewModels.TopNews;

    public class NewsController : BaseController
    {
        private INewsService newsService;
        private ISourceService sourceService;
        private ITopNewsService topNewsService;
        private IRssAtomService rssAtomService;

        public NewsController(INewsService newsService, ITopNewsService topNewsService, ISourceService sourceService, IRssAtomService rssAtomService)
        {
            this.newsService = newsService;
            this.sourceService = sourceService;
            this.topNewsService = topNewsService;
            this.rssAtomService = rssAtomService;
        }

        public async Task<IActionResult> AddAsync()
        {
            var sources = this.sourceService.GetAll();
            foreach (var source in sources)
            {
                await new GetNewPublicationsJob(this.newsService, this.sourceService, this.rssAtomService).StartAsync(source);
            }

            return this.Json("ok");
        }

        public IActionResult Details(int id)
        {
            var news = this.newsService.GetById<NewsViewModel>(id);
            var topNewsToday = this.topNewsService.GetAllFromToday<TopNewsViewModel>();
            var topNews = topNewsToday.GroupBy(x => x.SourceId).Select(g => g.ToList().Take(2)).SelectMany(x => x).ToList();
            var model = new NewsDetailsViewModel()
            {
                News = news,
                TopNews = topNews,
            };
            return news != null ? this.View(model) : this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SelectSources(List<int> sources)
        {
            var sourcesCount = this.sourceService.GetCount();

            // If no one is choosen will not set cookie.
            if (sources.Count == 0)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var sourcesJson = JsonSerializer.Serialize(sources);
            this.Response.Cookies.Append(
                "sourceSelect",
                sourcesJson,
                new CookieOptions { Secure = true, SameSite = SameSiteMode.Strict, IsEssential = true, HttpOnly = true });
            return this.RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 1200)]
        [HttpGet]
        public IActionResult Rss()
        {
            var descriptionText = "La información publicada en este sitio generalmente está disponible públicamente y es compartida por el autor original para su nueva publicación. Cada noticia en el sitio está vinculada al autor original de la publicación. Noticias En Original no es responsable del contenido de las noticias publicadas. Cuando utilice información del sitio, cite al autor original.";
            var feed = new SyndicationFeed("Noticias En Original", descriptionText, new Uri("https://noticiasenoriginal.azurewebsites.net/"), "https://noticiasenoriginal.azurewebsites.net/news/rss", DateTime.Now);
            var items = new List<SyndicationItem>();
            var news = this.newsService.GetLastN<NewsViewModel>(20);
            foreach (var item in news)
            {
                var newsUrl = item.OriginalUrl /*Url.Action("Article", "Blog", new { id = item.UrlSlug }, HttpContext.Request.Scheme)*/;
                var title = item.Title;
                var syndicationItem = new SyndicationItem(title, item.ShortContent, new Uri(newsUrl), item.Id.ToString(), item.Date);
                syndicationItem.PublishDate = new DateTimeOffset(item.Date);
                items.Add(syndicationItem);
            }

            feed.Items = items;

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true,
            };
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    var rssFormatter = new Rss20FeedFormatter(feed, false);
                    rssFormatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }

                return this.File(stream.ToArray(), "application/rss+xml; charset=utf-8");
            }
        }
    }
}
