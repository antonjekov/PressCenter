using AngleSharp;
using AngleSharp.Dom;
using PressCenter.Data.Models;
using PressCenter.Services.RssAtom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.SpanishGovernment
{
    public class MinisterioDeSanidad : BaseSource<IElement>
    {
        private readonly IBrowsingContext context;
        //private readonly IRssAtomService rssAtomService;

        public MinisterioDeSanidad(Source source /*, IRssAtomService rssAtomService*/) : base(source)
        {
            this.context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            //this.rssAtomService = rssAtomService;
        }

        public override async Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
        {
            var rssAtomService = new RssAtomService();
            var result = new List<RemoteNews>();
            var items = await rssAtomService.ReadAsync<News>(this.EntryPointUrl);
            foreach (var news in items)
            {
                var document = await this.context.OpenAsync(news.OriginalUrl);
                var element = document.QuerySelector("section.informacion");
                var remoteId = news.OriginalUrl.Split("id=").LastOrDefault();
                RemoteNews input =await GetInfoAsync(element);
                input.Title = news.Title;
                input.OriginalUrl = news.OriginalUrl;
                input.Date = news.Date;
                input.RemoteId = remoteId;
            }
            return result;
        }

        public override async Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var rssAtomService = new RssAtomService();
            var result = new List<RemoteNews>();
            var items = await rssAtomService.Read1Async(this.EntryPointUrl);
            foreach (var news in items)
            {
                var document = await this.context.OpenAsync(news.Link);
                var element = document.QuerySelector("section.informacion");
                var remoteId = news.Link.Split("id=").LastOrDefault();
                if (existingNewsRemoteIds.Contains(remoteId))
                {
                    continue;
                }
                RemoteNews input = await GetInfoAsync(element);
                input.Title = news.Title;
                input.OriginalUrl = news.Link;
                input.RemoteId = remoteId;
                input.Date = (DateTime)news.SpecificItem.Element.Descendants().First(x => x.Name.LocalName == "modified");
                result.Add(input);
            }
            return result;
        }

        protected override async Task<DateTime> GetDateAsync(IElement textHTML)
        {
            return DateTime.Now;
        }

        protected override async Task<string> GetImageUrlAsync(IElement textHTML)
        {
            return this.DefaultImageUrl;
        }

        protected override async Task<string> GetNewsContentAsync(IElement textHTML)
        {
            var newsContentParagraphs = textHTML.QuerySelectorAll("p");
            var sb = new StringBuilder();
            foreach (var item in newsContentParagraphs)
            {
                sb.AppendLine(item.TextContent);
            }
            var content = sb.ToString();
            return content;
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
