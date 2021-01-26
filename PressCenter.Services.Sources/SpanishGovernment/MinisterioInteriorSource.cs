

//namespace PressCenter.Services.Sources.SpanishGovernment
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Globalization;
//    using System.Linq;
//    using System.Net;
//    using System.Threading.Tasks;
//    using System.Web;
//    using AngleSharp;
//    using AngleSharp.Html.Dom;
//    using PressCenter.Data.Models;

//    public class MinisterioInteriorSource : BaseSource
//    {
//        private readonly string entryPointUrl;
//        private readonly string baseUrl;
//        private readonly string defaultImageUrl;

//        public MinisterioInteriorSource(Source source)
//        {
//            this.entryPointUrl = source.EntryPointUrl;
//            this.baseUrl = source.Url;
//            this.defaultImageUrl = source.DefaultImageUrl;
//        }
//        public override string EntryPointUrl => entryPointUrl;

//        public override string BaseUrl => baseUrl;

//        public override async Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
//        {
//            var result = new List<RemoteNews>();
//            //pass only from first 10 pages with news if you realy want all change logic
//            for (int i = 0; i < 10; i++)
//            {
//                var url = this.entryPointUrl + i;
//                var document = await this.Context.OpenAsync(url);
//                if (document==null)
//                {
//                    break;
//                }
//                var elementas = document.QuerySelectorAll("div.noticias");
//                foreach (var item in elementas)
//                {
//                    var textHTML = item.QuerySelector("div.texto");
//                    var dateInfo = textHTML.QuerySelector("div.right").TextContent;
//                    var date = DateTime.ParseExact(dateInfo.Substring(dateInfo.Length - 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);

//                    var thumbnailURLAddress = item.QuerySelector("div.imagen")?.QuerySelector("img").GetAttribute("src");
//                    string thumbnailURLFullAddress = null;
//                    if (thumbnailURLAddress != null)
//                    {
//                        thumbnailURLFullAddress = this.BaseUrl + thumbnailURLAddress;
//                    }
//                    else
//                    {
//                        thumbnailURLFullAddress = defaultImageUrl;
//                    }

//                    var titleInfo = textHTML.QuerySelector("h4.tit");
//                    var titleText = titleInfo.QuerySelector("p").TextContent;
//                    var newsUrl = titleInfo.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
//                    var shortText = textHTML.QuerySelector("div.textNoticia").TextContent;
//                    var fullNewHTML = await this.Context.OpenAsync(newsUrl);
//                    var remoteIdString = newsUrl.Split(new string[] { "/?" }, StringSplitOptions.None)?[0];
//                    var remoteId = remoteIdString.Substring(remoteIdString.Length - 8);
//                    var fullText = fullNewHTML.GetElementById("noticiaContenido").TextContent;
//                    var allImagesURL = fullNewHTML.QuerySelector("div.foto-gallery")?.QuerySelectorAll("img").Select(x => this.BaseUrl + x.GetAttribute("src")).ToList();

//                    var input = new RemoteNews(titleText, shortText, date, thumbnailURLFullAddress, remoteId)
//                    {
//                        Title = titleText,
//                        Content = shortText,
//                        ImageUrl = thumbnailURLFullAddress,
//                        OriginalUrl = newsUrl,
//                        Date = date,
//                    };
//                    result.Add(input);

//                }
//            }
//            return result;
//        }

//        public override async Task<IEnumerable<RemoteNews>> GetNewPublications(List<string> existingNewsRemoteIds)
//        {
//            var result = new List<RemoteNews>();
//            //pass only from first 10 pages with news if you realy want all change logic
//            var remoteIdExist = false;
//            for (int i = 1; i < 10; i++)
//            {
//                if (remoteIdExist)
//                {
//                    break;
//                }
//                var url = this.entryPointUrl + i;
//                var document = await this.Context.OpenAsync(url);
//                if (document == null)
//                {
//                    break;
//                }
//                var elementas = document.QuerySelectorAll("div.noticias");
//                foreach (var item in elementas)
//                {
//                    var textHTML = item.QuerySelector("div.texto");
//                    var dateInfo = textHTML.QuerySelector("div.right").TextContent;
//                    var date = DateTime.ParseExact(dateInfo.Substring(dateInfo.Length - 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);

//                    var thumbnailURLAddress = item.QuerySelector("div.imagen")?.QuerySelector("img").GetAttribute("src");
//                    string thumbnailURLFullAddress = null;
//                    if (thumbnailURLAddress != null)
//                    {
//                        thumbnailURLFullAddress = this.BaseUrl + thumbnailURLAddress;
//                    }
//                    else
//                    {
//                        thumbnailURLFullAddress = defaultImageUrl;
//                    }

//                    var titleInfo = textHTML.QuerySelector("h4.tit");
//                    var titleText = titleInfo.QuerySelector("p").TextContent;
//                    var newsUrl = titleInfo.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
//                    var shortText = textHTML.QuerySelector("div.textNoticia").TextContent;
//                    var fullNewHTML = await this.Context.OpenAsync(newsUrl);
//                    var remoteIdString = newsUrl.Split(new string[] { "/?" }, StringSplitOptions.None)?[0];
//                    var remoteId = remoteIdString.Substring(remoteIdString.Length - 8);
//                    var fullText = fullNewHTML.GetElementById("noticiaContenido").TextContent;
//                    var allImagesURL = fullNewHTML.QuerySelector("div.foto-gallery")?.QuerySelectorAll("img").Select(x => this.BaseUrl + x.GetAttribute("src")).ToList();

//                    var input = new RemoteNews(titleText, shortText, date, thumbnailURLFullAddress, remoteId)
//                    {
//                        Title = titleText,
//                        Content = shortText,
//                        ImageUrl = thumbnailURLFullAddress,
//                        OriginalUrl = newsUrl,
//                        Date=date
//                    };
//                    if (existingNewsRemoteIds.Contains(remoteId))
//                    {
//                        remoteIdExist = true;
//                        break;
//                    }
//                    result.Add(input);
//                }
//            }
//            return result;
//        }
//    }
//}
