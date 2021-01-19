using PressCenter.Data;
using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using PressCenter.Services.Data;
using PressCenter.Services.Sources.SpanishGovernment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task StartAsync(Source source)
        {
            //To DO with reflection create instance of nesesary source
            var newPublications = await new MinisterioInteriorSource(source.EntryPointUrl, source.Url).GetNewPublications(this.resourceIds);
            foreach (var publication in newPublications)
            {
                await this.newsService.AddAsync(publication, source.Id);
            }
        }
    }
}
