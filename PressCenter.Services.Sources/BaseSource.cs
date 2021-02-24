namespace PressCenter.Services.Sources
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AngleSharp;
    using PressCenter.Data.Models;
    using PressCenter.Services.RssAtom;

    public abstract class BaseSource<T>
    {
        protected BaseSource(Source source, IRssAtomService rssAtomService)
        {
            this.BaseUrl = source.Url;
            this.EntryPointUrl = source.EntryPointUrl;
            this.DefaultImageUrl = source.DefaultImageUrl;
            this.RssAtomService = rssAtomService;
        }

        protected string EntryPointUrl { get; }
        protected string DefaultImageUrl { get; }
        public IRssAtomService RssAtomService { get; }
        protected string BaseUrl { get; }
        
        protected virtual async Task<RemoteNews> GetInfoAsync(T textHTML)
        {
            var date = await GetDateAsync(textHTML);
            var imageUrl = await GetImageUrlAsync(textHTML);
            var title = (await GetTitleAsync(textHTML)).Trim();
            var originalUrl = await GetOriginalUrlAsync(textHTML);
            var content = (await GetNewsContentAsync(textHTML)).Trim();
            var remoteId = await GetRemoteIdAsync(textHTML);
            var news = new RemoteNews(title, content, date, imageUrl, remoteId, originalUrl);
            return news;
        }

        protected abstract Task<DateTime> GetDateAsync(T textHTML);
        protected abstract Task<string> GetImageUrlAsync(T textHTML);
        protected abstract Task<string> GetTitleAsync(T textHTML);
        protected abstract Task<string> GetNewsContentAsync(T textHTML);
        protected abstract Task<string> GetOriginalUrlAsync(T textHTML);
        protected abstract Task<string> GetRemoteIdAsync(T textHTML);
        public abstract Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds);
    }
}
