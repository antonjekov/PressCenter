using PressCenter.Data.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources
{
    public abstract class BaseTopNewsSource
    {
        private int sourceId;

        protected BaseTopNewsSource(TopNewsSource source)
        {
            this.sourceId = source.Id;
            this.Url = source.Url;
        }
        public string Url { get; }

        protected virtual async Task<TopNews> GetInfoAsync(ElementHandle textHTML)
        {
            var imageUrl = await GetImageUrlAsync(textHTML);
            var title = await GetTitleAsync(textHTML);
            var originalUrl = await GetOriginalUrlAsync(textHTML);
            var remoteId = await GetRemoteIdAsync(textHTML);
            var news = new TopNews(title,imageUrl,originalUrl,remoteId);
            return news;
        }

        protected abstract Task<string> GetImageUrlAsync(ElementHandle textHTML);
        protected abstract Task<string> GetTitleAsync(ElementHandle textHTML);
        protected abstract Task<string> GetOriginalUrlAsync(ElementHandle textHTML);
        protected abstract Task<string> GetRemoteIdAsync(ElementHandle textHTML);
        public abstract Task<IEnumerable<TopNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds);
        public abstract Task<IEnumerable<TopNews>> GetAllPublicationsAsync();
    }
}
