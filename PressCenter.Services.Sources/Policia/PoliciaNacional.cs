﻿using AngleSharp;
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
    public class PoliciaNacional : BaseSource
    {
        public PoliciaNacional(Source source) : base(source)
        {
        }

        public override async Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
        {
            var result = new List<RemoteNews>();
            var year = DateTime.Now.Year;
            // will get only last 10 pages with news from current year for more news change logic
            for (int i = 1; i < 10; i++)
            {
                var url = String.Format(this.EntryPointUrl, year, i);
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var elements = document.QuerySelectorAll("div.contenido_nota_prensa");
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
            var year = DateTime.Now.Year;
            // will get only last 10 pages with news from current year for more news change logic
            var remoteIdExist = false;
            for (int i = 1; i < 10; i++)
            {
                if (remoteIdExist)
                {
                    break;
                }
                var url = String.Format(this.EntryPointUrl, year, i);
                var document = await this.Context.OpenAsync(url);
                if (document == null)
                {
                    break;
                }
                var elements = document.QuerySelectorAll("div.contenido_nota_prensa");
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

        protected override DateTime GetDate(IElement textHTML)
        {
            var dateInfo = textHTML.QuerySelector("div.fecha_nota_prensa").TextContent;
            var date = DateTime.ParseExact(dateInfo.Substring(0, 10), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            return date;
        }

        protected override string GetImageUrl(IElement textHTML)
        {
            var imageUrl = string.Empty;
            var imageUrlPath = textHTML
               .QuerySelector("div.imagen_nota_prensa")?
               .QuerySelector("img")
               .GetAttribute("src");

            if (imageUrlPath != null)
            {
                imageUrl = this.BaseUrl + "prensa" + imageUrlPath.Substring(2);
            }
            else
            {
                imageUrl = this.DefaultImageUrl;
            }
            return imageUrl;
        }

        protected override async Task<string> GetNewsContentAsync(IElement textHTML)
        {
            //var newsContent = textHTML.QuerySelector("div.antetitulo_nota_prensa").TextContent;
            //return newsContent;

            var pageFullNews = await this.Context.OpenAsync(this.GetOriginalUrl(textHTML));
            var newsContentParagraphs = pageFullNews.GetElementById("contenido_nota");
            newsContentParagraphs.QuerySelector("span.antetitulo").Remove();
            newsContentParagraphs.QuerySelector("span.titulo").Remove();
            
            return newsContentParagraphs.TextContent;
        }

        protected override string GetOriginalUrl(IElement textHTML)
        {
            var titleInfo = textHTML.QuerySelector("div.titular_nota_prensa");
            var newsUrl = titleInfo.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
            return newsUrl;
        }

        protected override string GetRemoteId(IElement textHTML)
        {
            return this.GetTitle(textHTML);
        }

        protected override string GetTitle(IElement textHTML)
        {
            var titleText = textHTML.QuerySelector("div.titular_nota_prensa").TextContent;
            return titleText;
        }
    }
}
