using AngleSharp;
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
    public class PoliciaNacional : BaseSource
    {
        private string entryPointUrl;
        private string baseUrl;

        public PoliciaNacional(Source source)
        {
            this.entryPointUrl = source.EntryPointUrl;
            this.baseUrl = source.Url;
        }
        public override string EntryPointUrl => this.entryPointUrl;

        public override string BaseUrl => this.baseUrl;

        public override async Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
        {
            var result = new List<RemoteNews>();
            var year = DateTime.Now.Year;
            // will get only last 10 pages with news from current year for more news change logic
            for (int i = 1; i < 10; i++)
            {
                var url = this.entryPointUrl + $"y={year}&_pagi_pg={i}";
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var elementas = document.QuerySelectorAll("div.contenido_nota_prensa");
                foreach (var item in elementas)
                {
                    var textHTML = item.QuerySelector("div.texto_nota_prensa");
                    RemoteNews input = GetInfo(item, textHTML);
                    result.Add(input);
                }
            }
            return result;
        }

        
        public override async Task<IEnumerable<RemoteNews>> GetNewPublications(List<string> existingNewsRemoteIds)
        {
            var result = new List<RemoteNews>();
            var year = DateTime.Now.Year;
            // will get only last 10 pages with news from current year for more news change logic
            var remoteIdExist = false;
            for (int i = 1; i < 10; i++)
            {
                if (remoteIdExist)
                {
                    break;
                }
                var url = this.entryPointUrl + $"y={year}&_pagi_pg={i}";
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var elementas = document.QuerySelectorAll("div.contenido_nota_prensa");
                foreach (var item in elementas)
                {
                    var textHTML = item.QuerySelector("div.texto_nota_prensa");
                    RemoteNews input = GetInfo(item, textHTML);
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

        private RemoteNews GetInfo(AngleSharp.Dom.IElement item, AngleSharp.Dom.IElement textHTML)
        {
            var dateInfo = textHTML.QuerySelector("div.fecha_nota_prensa").TextContent;
            var date = DateTime.ParseExact(dateInfo.Substring(0, 10), "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var thumbnailURLAddress = item
                .QuerySelector("div.imagen_nota_prensa")?
                .QuerySelector("img")
                .GetAttribute("src");

            string thumbnailURLFullAddress = null;
            if (thumbnailURLAddress != null)
            {
                thumbnailURLFullAddress = this.BaseUrl + "prensa"+ thumbnailURLAddress.Substring(2);
            }
            var titleInfo = item.QuerySelector("div.titular_nota_prensa");
            var titleText = titleInfo.TextContent;
            var newsUrl = titleInfo.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;

            var shortText = item.QuerySelector("div.antetitulo_nota_prensa").TextContent;
            var remoteId = titleText;

            var input = new RemoteNews(titleText, shortText, date, thumbnailURLFullAddress, remoteId)
            {
                Title = titleText,
                Content = shortText,
                ImageUrl = thumbnailURLFullAddress,
                OriginalUrl = newsUrl,
                Date = date,
                RemoteId = remoteId
            };
            return input;
        }
    }
}
