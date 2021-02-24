using AngleSharp;
using AngleSharp.Dom;
using PressCenter.Data.Models;
using PressCenter.Services.RssAtom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.Catalunya
{
    public class GeneralitatDeCatalunya : BaseSource<IElement>
    {
        private readonly IBrowsingContext context;
        private readonly IRssAtomService rssAtomService;

        public GeneralitatDeCatalunya(Source source, IRssAtomService rssAtomService) : base(source, rssAtomService)
        {
            this.context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            this.rssAtomService = rssAtomService;
        }

        public override async Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<RemoteNews>();
            var items = await this.rssAtomService.ReadAsync(this.EntryPointUrl);
            foreach (var news in items)
            {
                string remoteId = (string)news.SpecificItem.Element.Descendants().First(x => x.Name.LocalName == "guid");
                if (existingNewsRemoteIds.Contains(remoteId))
                {
                    continue;
                }
                var document = await this.context.OpenAsync(news.Link);
                var element = document.QuerySelector("#detall");                
                RemoteNews input = await GetInfoAsync(element);
                input.Title = news.Title;
                input.OriginalUrl = news.Link;
                input.RemoteId = remoteId;
                input.Date = (DateTime)news.SpecificItem.Element.Descendants().First(x => x.Name.LocalName == "pubDate");
                result.Add(input);
            }
            return result;
        }

        protected override async Task<DateTime> GetDateAsync(IElement textHTML)
        {
            return DateTime.UtcNow;
        }

        protected override async Task<string> GetImageUrlAsync(IElement textHTML)
        {
            var imageUrlInfo = textHTML.QuerySelectorAll("img").FirstOrDefault();
            string imageUrl;
            if (imageUrlInfo != null)
            {
                imageUrl =this.BaseUrl+ imageUrlInfo.GetAttribute("src");
            }
            else
            {
                imageUrl = this.DefaultImageUrl;
            }
            return imageUrl;
        }

        protected override async Task<string> GetNewsContentAsync(IElement textHTML)
        {
            var sb = new StringBuilder();
            var newsDescription = textHTML.QuerySelector("p.noticia_descp").TextContent;
            sb.AppendLine(newsDescription);
            sb.AppendLine();

            var allElementsDiv = textHTML.QuerySelector("div.basic_text_peq").QuerySelectorAll("div");
            if (allElementsDiv.Count() > 0)
            {
                foreach (var item in allElementsDiv)
                {
                    sb.AppendLine(item.TextContent);
                }

            }
            else
            {
                var allElementsText = textHTML.QuerySelector("div.basic_text_peq").TextContent;
                sb.AppendLine(allElementsText);
            }
            var allText = sb.ToString();
            return allText;
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
