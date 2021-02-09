namespace PressCenter.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;
    using PressCenter.Services.Mapping;

    public class TopNewsService : ITopNewsService
    {
        private readonly IDeletableEntityRepository<TopNews> newsRepository;

        public TopNewsService(IDeletableEntityRepository<TopNews> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        public async Task AddAsync(Sources.TopNews topNews, int sourceId)
        {
            var news = new TopNews()
            {
                SourceId = sourceId,
                RemoteId = topNews.RemoteId,
                Title = topNews.Title,
                ImageUrl = topNews.ImageUrl,
                OriginalUrl = topNews.OriginalUrl,
            };
            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();
        }

        public int Count() => this.newsRepository.AllAsNoTracking().Count();

        public IEnumerable<T> GetAll<T>()
        {
            return this.newsRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();
        }

        public T GetById<T>(int id)
        {
            return this.newsRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public List<string> GetAllRemoteIds()
        {
            var remoteIds = this.newsRepository.AllAsNoTracking().Select(x => x.RemoteId).ToList();
            return remoteIds;
        }
    }
}
