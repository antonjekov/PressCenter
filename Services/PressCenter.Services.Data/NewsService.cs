namespace PressCenter.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;
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
            };
            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();
        }

        public int Count() => this.newsRepository.AllAsNoTracking().Count();
    }
}
