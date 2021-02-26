using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using PressCenter.Data.Models;
using PressCenter.Services.RssAtom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.Catalunya
{
    public class MossosEsquadra : BaseSource<IElement>
    {
        private readonly IBrowsingContext context;
        private readonly IRssAtomService rssAtomService;

        public MossosEsquadra(Source source, IRssAtomService rssAtomService) : base(source, rssAtomService)
        {
            this.context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            this.rssAtomService = rssAtomService;
        }

        public override async Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<RemoteNews>();
            var document = await this.context.OpenAsync(this.EntryPointUrl);
            var items = document.QuerySelector("#main").QuerySelectorAll("div.destacat_noticies_cont");

            foreach (var news in items)
            {
                var id = await this.GetRemoteIdAsync(news);
                if (existingNewsRemoteIds.Contains(id))
                {
                    continue;
                }
                RemoteNews input = await GetInfoAsync(news);
                result.Add(input);
            }
            return result;
        }

        protected override async Task<DateTime> GetDateAsync(IElement textHTML)
        {
            var dateInfo = textHTML.QuerySelector("span").TextContent;
            var date = DateTime.ParseExact(dateInfo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return date;
        }

        protected override async Task<string> GetImageUrlAsync(IElement textHTML)
        {
            string imageUrl;
            var imageElement = textHTML.QuerySelector("div.notc_img")?.QuerySelector("img");

            if (imageElement != null)
            {
                imageUrl = imageElement.GetAttribute("src");
            }
            else
            {
                imageUrl = this.DefaultImageUrl;
            }
            return imageUrl;
        }


        protected override async Task<string> GetNewsContentAsync(IElement textHTML)
        {
            var url = await this.GetOriginalUrlAsync(textHTML);
            var document =await this.context.OpenAsync(url);
            var sb = new StringBuilder();
            var firstLines = document.QuerySelector("div.entradeta");
            if (firstLines != null)
            {
                sb.AppendLine(firstLines.TextContent.Trim());
                sb.AppendLine();
            }
            var paragraphs = document.QuerySelector("div.basic_text_peq")?.QuerySelectorAll("p");
            if (paragraphs != null && paragraphs.Count() > 0)
            {
                foreach (var item in paragraphs)
                {
                    sb.AppendLine(item.TextContent);
                    sb.AppendLine();
                }
            }
            else
            {
                var content = document.QuerySelector("div.basic_text_peq");
                if (content != null)
                {
                    sb.AppendLine(content.TextContent);
                }
            }
            return sb.ToString();
        }

        protected override async Task<string> GetOriginalUrlAsync(IElement textHTML)
        {
            var titleInfo = textHTML.QuerySelector("h3");
            var newsUrl = titleInfo.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
            return newsUrl;
        }

        protected override async Task<string> GetRemoteIdAsync(IElement textHTML)
        {
            var url = await this.GetOriginalUrlAsync(textHTML);
            var remoteId = url.Split("?id=")?
                .LastOrDefault();

            return remoteId;
        }

        protected override async Task<string> GetTitleAsync(IElement textHTML)
        {
            var title = textHTML.QuerySelector("h3").TextContent;
            return title;
        }
    }
}
