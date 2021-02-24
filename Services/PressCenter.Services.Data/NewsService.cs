namespace PressCenter.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;
    using PressCenter.Services.Mapping;
    using PressCenter.Services.Sources;

    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> newsRepository;

        public NewsService(IDeletableEntityRepository<News> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        public async Task AddAsync(RemoteNews remoteNews, int sourceId)
        {
            var news = new News()
            {
                SourceId = sourceId,
                RemoteId = remoteNews.RemoteId,
                Title = remoteNews.Title,
                Content = remoteNews.Content,
                ImageUrl = remoteNews.ImageUrl,
                OriginalUrl = remoteNews.OriginalUrl,
                Date = remoteNews.Date,
            };
            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();
        }

        public T GetById<T>(int id)
        {
            return this.newsRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public IEnumerable<News> GetAll()
        {
            return this.newsRepository.All()
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.CreatedOn)
                .ToList();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.newsRepository.All()
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage)
        {
            return this.newsRepository.All()
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage, List<int> sourceIds)
        {
            return this.newsRepository.All()
                .Where(x => sourceIds.Contains((int)x.SourceId))
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public int Count() => this.newsRepository.AllAsNoTracking().Count();
  
        public int Count(List<int> sourceIds) => this.newsRepository.AllAsNoTracking().Where(x => sourceIds.Contains((int)x.SourceId)).Count();

        public int NewsTodayCount()
        {
            var result = this.newsRepository.AllAsNoTracking()
                .Where(x => x.Date.Date == DateTime.UtcNow.Date)
                .Count();
            return result;
        }

        public int NewsTodayCount(List<int> sourceIds)
        {
            var result = this.newsRepository.AllAsNoTracking()
                .Where(x => x.Date.Date == DateTime.UtcNow.Date)
                .Where(x => sourceIds.Contains((int)x.SourceId))
                .Count();
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var item = this.newsRepository.All().FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                this.newsRepository.Delete(item);
                await this.newsRepository.SaveChangesAsync();
            }
        }

        public List<string> GetAllRemoteIds()
        {
            var remoteIds = this.newsRepository.AllAsNoTracking().Select(x => x.RemoteId).ToList();
            return remoteIds;
        }

        public List<string> GetAllRemoteIdsBySourceId(int id)
        {
            var remoteIds = this.newsRepository.AllAsNoTracking().Where(x => x.SourceId == id).Select(x => x.RemoteId).ToList();
            return remoteIds;
        }

        public IEnumerable<T> GetLastN<T>(int n)
        {
            return this.newsRepository.All()
               .OrderByDescending(x => x.Date)
               .ThenByDescending(x => x.CreatedOn)
               .Take(n)
               .To<T>()
               .ToList();
        }
    }
}
