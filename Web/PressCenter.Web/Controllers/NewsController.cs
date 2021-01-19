using Microsoft.AspNetCore.Mvc;
using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using PressCenter.Services.CronJobs;
using PressCenter.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressCenter.Web.Controllers
{
    public class NewsController : BaseController
    {
        private INewsService newsService;
        private IDeletableEntityRepository<News> newsRepository;
        private ISourceService sourceService;

        public NewsController(INewsService newsService, IDeletableEntityRepository<News> newsRepository, ISourceService sourceService)
        {
            this.newsService = newsService;
            this.newsRepository = newsRepository;
            this.sourceService = sourceService;
        }

        public async Task<IActionResult> AddAsync()
        {
            var sources = this.sourceService.GetAll();
            foreach (var source in sources)
            {
                await new GetNewPublicationsJob(this.newsRepository, this.newsService).StartAsync(source);
            }

            return this.Json("ok");
        }
    }
}
