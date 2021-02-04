using Hangfire;
using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using PressCenter.Services.Data;
using PressCenter.Services.Sources;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.CronJobs
{
    public class GetNewTopNewsJob
    {
        private readonly IDeletableEntityRepository<PressCenter.Data.Models.TopNews> newsRepository;
        private readonly List<string> resourceIds;
        private readonly ITopNewsService newsService;

        public GetNewTopNewsJob(IDeletableEntityRepository<PressCenter.Data.Models.TopNews> newsRepository, ITopNewsService newsService)
        {
            this.newsRepository = newsRepository;
            this.resourceIds = newsRepository.AllAsNoTracking().Select(x => x.RemoteId).ToList();
            this.newsService = newsService;
        }

        [AutomaticRetry(Attempts = 2)]
        public async Task StartAsync(TopNewsSource source)
        {
            var type = typeof(BaseTopNewsSource).Assembly.GetType(source.TypeName);
            if (type == null)
            {
                throw new Exception($"Type \"{source.TypeName}\" not found!");
            }

            var instance = (BaseTopNewsSource)Activator.CreateInstance(type, source);
            if (instance == null)
            {
                throw new Exception($"Unable to create {typeof(BaseTopNewsSource).Name} instance from \"{source.TypeName}\"!");
            }

            var newPublications = await instance.GetNewPublicationsAsync(this.resourceIds);
            foreach (var publication in newPublications)
            {
                await this.newsService.AddAsync(publication, source.Id);
            }
        }
    }
}
