using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using PressCenter.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.Policia
{
    public class DGT : BaseSource<IElement>
    {
        public DGT(Source source) : base(source)
        {
        }
        public override async Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
        {
            var result = new List<RemoteNews>();
            var year = DateTime.Now.Year;
            for (int i = 1; i < 10; i++)
            {
                var url = String.Format(this.EntryPointUrl, year);
                if (i > 1)
                {
                    url += $"index-paginacion-{i.ToString().PadLeft(3, '0')}.shtml";
                }
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var elements = document.QuerySelector("section.stcols").QuerySelectorAll("article");
                foreach (var item in elements)
                {
                    RemoteNews input = await GetInfoAsync(item);
                    result.Add(input);
                }
            }
            return result;
        }

        public override async Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<RemoteNews>();
            var year = DateTime.Now.Year;
            var remoteIdExist = false;
            for (int i = 1; i < 10; i++)
            {
                if (remoteIdExist)
                {
                    break;
                }
                var url = String.Format(this.EntryPointUrl, year);
                if (i > 1)
                {
                    url += $"index-paginacion-{i.ToString().PadLeft(3, '0')}.shtml";
                }
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var sectionNews = document.QuerySelector("section.stcols");
                if (sectionNews == null)
                {
                    break;
                }
                var elements = sectionNews.QuerySelectorAll("article");
                foreach (var item in elements)
                {
                    RemoteNews input =await GetInfoAsync(item);
                    if (existingNewsRemoteIds.Contains(input.RemoteId))
                    {
                        remoteIdExist = true;
                        break;
                    }
                    result.Add(input);
                }
            }
            return result;
        }

        protected override async Task<DateTime> GetDateAsync(IElement textHTML)
        {
            var dateInfo = textHTML.QuerySelector("time").TextContent;
            var date = DateTime.ParseExact(dateInfo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return date;
        }

        protected override async Task<string> GetImageUrlAsync(IElement textHTML)
        {
            var imageUrlPath = textHTML
               .QuerySelector("figure")?
               .QuerySelector("img")
               .GetAttribute("src");

            string imageUrl;
            if (imageUrlPath != null)
            {
                imageUrl = this.BaseUrl + imageUrlPath;
            }
            else
            {
                imageUrl = this.DefaultImageUrl;
            }
            return imageUrl;
        }

        protected override async Task<string> GetNewsContentAsync(IElement textHTML)
        {
            //var newsContent = textHTML.QuerySelector("p").TextContent;
            //return newsContent;
            var pageFullNews = await this.Context.OpenAsync(await this.GetOriginalUrlAsync(textHTML));
            var newsContentParagraphs = pageFullNews.QuerySelector("section.notap").QuerySelectorAll("p");
            newsContentParagraphs[0].Remove();
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
            var newsUrl = textHTML.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
            return newsUrl;
        }

        protected override async Task<string> GetRemoteIdAsync(IElement textHTML)
        {
            return await this.GetTitleAsync(textHTML);
        }

        protected override async Task<string> GetTitleAsync(IElement textHTML)
        {
            var titleText = textHTML.QuerySelector("h4").TextContent;
            return titleText;
        }
    }
}
