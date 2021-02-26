namespace PressCenter.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;
    using PressCenter.Services.Mapping;

    public class TopNewsService : ITopNewsService
    {
        private readonly IDeletableEntityRepository<TopNews> newsRepository;
        private readonly IDataValidationService dataValidationService;

        public TopNewsService(IDeletableEntityRepository<TopNews> newsRepository, IDataValidationService dataValidationService)
        {
            this.newsRepository = newsRepository;
            this.dataValidationService = dataValidationService;
        }

        public async Task AddAsync(ITopNews topNews, int sourceId)
        {
            if (!this.dataValidationService.ValidateTopNews(topNews))
            {
                throw new ValidationException($"TopNews from TopNewsSourceId: {sourceId}, with OriginalUrl: {topNews.OriginalUrl} don't pass validation!");
            }

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

        public IEnumerable<T> GetAllFromToday<T>()
        {
            return this.newsRepository.All()
                .Where(x => x.CreatedOn >= DateTime.Today)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();
        }

        public List<string> GetAllRemoteIdsBySourceId(int id)
        {
            var remoteIds = this.newsRepository.AllAsNoTracking().Where(x => x.SourceId == id).Select(x => x.RemoteId).ToList();
            return remoteIds;
        }
    }
}
