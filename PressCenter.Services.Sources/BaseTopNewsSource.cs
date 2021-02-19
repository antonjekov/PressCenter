using PressCenter.Data.Models;
using PressCenter.Services.RssAtom;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources
{
    public abstract class BaseTopNewsSource<T>
    {
        private int sourceId;

        protected BaseTopNewsSource(TopNewsSource source, IRssAtomService rssAtomService)
        {
            this.sourceId = source.Id;
            this.Url = source.Url;
            this.RssAtomService = rssAtomService;
        }
        
        public string Url { get; }
        public IRssAtomService RssAtomService { get; }

        protected virtual async Task<TopNews> GetInfoAsync(T textHTML)
        {
            var imageUrl = await GetImageUrlAsync(textHTML);
            var title = await GetTitleAsync(textHTML);
            var originalUrl = await GetOriginalUrlAsync(textHTML);
            var remoteId = await GetRemoteIdAsync(textHTML);
            var news = new TopNews(title,imageUrl,originalUrl,remoteId);
            return news;
        }

        protected abstract Task<string> GetImageUrlAsync(T textHTML);
        protected abstract Task<string> GetTitleAsync(T textHTML);
        protected abstract Task<string> GetOriginalUrlAsync(T textHTML);
        protected abstract Task<string> GetRemoteIdAsync(T textHTML);
        public abstract Task<IEnumerable<TopNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds);        
    }
}
