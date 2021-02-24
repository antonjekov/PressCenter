using AngleSharp;
using AngleSharp.Dom;
using PressCenter.Data.Models;
using PressCenter.Services.RssAtom;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.Medias
{
    public class ElMundo : BaseTopNewsSource<IElement>
    {
        private readonly IBrowsingContext context;

        public ElMundo(TopNewsSource source, IRssAtomService rssAtomService) : base(source, rssAtomService)
        {
            this.context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        }

        public override async Task<IEnumerable<TopNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<TopNews>();
            var elements = await this.RssAtomService.ReadAsync(this.Url);
            foreach (var news in elements)
            {
                if (existingNewsRemoteIds.Contains(news.Link))
                {
                    continue;
                }
                var document = await this.context.OpenAsync(news.Link);
                var textHtml = document.QuerySelector("article");
                var item = await GetInfoAsync(textHtml);
                item.RemoteId = news.Link;
                item.Title = news.Title;
                item.OriginalUrl = news.Link;
                result.Add(item);
            }
            return result;
        }

        protected override async Task<string> GetImageUrlAsync(IElement textHTML)
        {
            var imgUrl = textHTML.QuerySelector("picture")?.QuerySelector("img")?.GetAttribute("src");
            if (imgUrl==null)
            {
                return String.Empty;
            }
            return imgUrl;
        }

        protected override async Task<string> GetOriginalUrlAsync(IElement textHTML)
        {
            return String.Empty;
        }

        protected override async Task<string> GetRemoteIdAsync(IElement textHTML)
        {
            return String.Empty;
        }

        protected override async Task<string> GetTitleAsync(IElement textHTML)
        {
            return String.Empty;
        }
    }
}
