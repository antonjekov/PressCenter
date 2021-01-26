using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using PressCenter.Data.Models;

namespace PressCenter.Services.Sources
{
    public abstract class BaseSource
    {
        protected BaseSource (Source source)
        {
            this.Context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            this.BaseUrl = source.Url;
            this.EntryPointUrl = source.EntryPointUrl;
            this.DefaultImageUrl = source.DefaultImageUrl;
        }

        protected string EntryPointUrl { get; }
        protected string DefaultImageUrl { get; }
        protected string BaseUrl { get; }
        protected IBrowsingContext Context { get; }

        protected virtual async Task<RemoteNews> GetInfoAsync(AngleSharp.Dom.IElement textHTML)
        {
            var date = GetDate(textHTML);
            var imageUrl = GetImageUrl(textHTML);
            var title = GetTitle(textHTML);
            var originalUrl = GetOriginalUrl(textHTML);
            var content =(await GetNewsContentAsync(textHTML)).Trim();
            var remoteId = GetRemoteId(textHTML);
            var news = new RemoteNews(title, content, date, imageUrl, remoteId, originalUrl);
            return news;
        }

        protected abstract DateTime GetDate(AngleSharp.Dom.IElement textHTML);
        protected abstract string GetImageUrl(AngleSharp.Dom.IElement textHTML);
        protected abstract string GetTitle(AngleSharp.Dom.IElement textHTML);
        protected abstract Task<string> GetNewsContentAsync(AngleSharp.Dom.IElement textHTML);
        protected abstract string GetOriginalUrl(AngleSharp.Dom.IElement textHTML);
        protected abstract string GetRemoteId(AngleSharp.Dom.IElement textHTML);
        public abstract Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds);
        public abstract Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync();
    }
}
