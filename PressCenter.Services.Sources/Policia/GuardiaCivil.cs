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
    public class GuardiaCivil : BaseSource<IElement>
    {
        public GuardiaCivil(Source source) : base(source)
        {
        }
        public override async Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
        {
            var result = new List<RemoteNews>();
            for (int i = 1; i < 10; i++)
            {
                var url = String.Format(this.EntryPointUrl, i);
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var elements = document.QuerySelector("div.list-block").QuerySelectorAll("div.list-item");
                foreach (var item in elements)
                {
                    RemoteNews input =await GetInfoAsync(item);
                    result.Add(input);
                }
            }
            return result;
        }

        public override async Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<RemoteNews>();
            var remoteIdExist = false;
            for (int i = 1; i < 10; i++)
            {
                if (remoteIdExist)
                {
                    break;
                }
                var url = String.Format(this.EntryPointUrl, i);
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var elements = document.QuerySelector("div.list-block").QuerySelectorAll("div.list-item");
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
            var dateInfo = textHTML.QuerySelector("span.fecha-noticia").TextContent;
            var date = DateTime.ParseExact(dateInfo, "dd/MM/yy", CultureInfo.InvariantCulture);
            return date;
        }

        protected override async Task<string> GetImageUrlAsync(IElement textHTML)
        {
            var imageUrlPath = textHTML
               .QuerySelector("div.image-left")?
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

        protected override async Task<string> GetTitleAsync(IElement textHTML)
        {
            var titleInfo = textHTML.QuerySelector("div.titular-noticia");
            var titleText = titleInfo.TextContent;
            return titleText;
        }

        protected override async Task<string> GetOriginalUrlAsync(IElement textHTML)
        {
            var titleInfo = textHTML.QuerySelector("div.titular-noticia");
            var newsUrl = titleInfo.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
            return newsUrl;
        }

        protected override async Task<string> GetNewsContentAsync(IElement textHTML)
        {
            // to get full text
            var pageFullNews =await this.Context.OpenAsync(await this.GetOriginalUrlAsync(textHTML));
            var newsContentParagraphs = pageFullNews.QuerySelector("div.text-noticia");
            newsContentParagraphs.QuerySelector("p.titular-noticia").Remove();
            var scripts = newsContentParagraphs.QuerySelectorAll("script");
            foreach (var item in scripts)
            {
                item.Remove();
            }

            return newsContentParagraphs.TextContent;
        }

        protected override async Task<string> GetRemoteIdAsync(IElement textHTML)
        {
            var url = await this.GetOriginalUrlAsync(textHTML);
            var remoteId =   url.Split('/')
                .LastOrDefault()?
                .Split('.')?[0];
            return remoteId;
        }        
    }
}
