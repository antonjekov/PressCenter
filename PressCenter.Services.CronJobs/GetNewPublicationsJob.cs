using Hangfire;
using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using PressCenter.Services.Data;
using PressCenter.Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressCenter.Services.CronJobs
{
    public class GetNewPublicationsJob
    {
        private readonly IDeletableEntityRepository<News> newsRepository;
        private readonly List<string> resourceIds;
        private readonly INewsService newsService;

        public GetNewPublicationsJob(IDeletableEntityRepository<News> newsRepository, INewsService newsService)
        {
            this.newsRepository = newsRepository;
            this.resourceIds = newsRepository.AllAsNoTracking().Select(x => x.RemoteId).ToList();
            this.newsService = newsService;
        }

        [AutomaticRetry(Attempts = 2)]
        public async Task StartAsync(Source source)
        {
            var type = typeof(BaseSource).Assembly.GetType(source.TypeName);
            if (type == null)
            {
                throw new Exception($"Type \"{source.TypeName}\" not found!");
            }

            var instance = (BaseSource)Activator.CreateInstance(type, source);
            if (instance == null)
            {
                throw new Exception($"Unable to create {typeof(BaseSource).Name} instance from \"{source.TypeName}\"!");
            }
            var newPublications = await instance.GetNewPublicationsAsync(this.resourceIds);
            foreach (var publication in newPublications)
            {
                await this.newsService.AddAsync(publication, source.Id);
            }
        }
    }
}
