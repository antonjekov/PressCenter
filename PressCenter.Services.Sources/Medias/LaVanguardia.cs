using AngleSharp;
using AngleSharp.Dom;
using PressCenter.Data.Models;
using PressCenter.Services.RssAtom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace PressCenter.Services.Sources.Medias
{
    public class LaVanguardia : BaseTopNewsSource<IElement>
    {
        private readonly IBrowsingContext context;

        public LaVanguardia(TopNewsSource source, IRssAtomService rssAtomService) : base(source, rssAtomService)
        {
            this.context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        }

        public override async Task<IEnumerable<TopNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<TopNews>();
            var doc = XDocument.Load(this.Url);
            var root = doc.Root;
            var newsItems = root.Elements("NewsItem");
            foreach (var newsItem in newsItems)
            {
                try
                {
                    var dateInfo = newsItem.Element("NewsManagement").Element("ThisRevisionCreated");
                    DateTime date = DateTimeOffset.ParseExact((string)dateInfo, "yyyy-MM-ddTH:mm:sszz", CultureInfo.InvariantCulture).UtcDateTime;
                    if (date <= DateTime.Now.AddDays(-1))
                    {
                        continue;
                    }
                    var title = HttpUtility.HtmlDecode(newsItem.Element("NewsLines").Element("HeadLine").Value); 
                    var originalUrl = newsItem.Element("NewsLines").Element("DeriveredFrom").Value;
                    var remoteID = newsItem.Element("NewsComponent").Attribute("Duid").Value;
                    var imageUrl = newsItem.Element("NewsComponent")
                        .Element("NewsComponent")
                        .Element("NewsComponent")
                        .Element("ContentItem")
                        .Element("DataContent")
                        .Element("nitf")
                        .Element("body")
                        .Element("body.content")
                        .Element("media")
                        .Element("media-reference")
                        .Attribute("source").Value;
                    var item = new TopNews(title, imageUrl, originalUrl, remoteID);
                    result.Add(item);
                }
                catch (Exception)
                {
                    continue;
                }                
            }
            result.Reverse();
            return result;
        }

        protected override Task<string> GetImageUrlAsync(IElement textHTML)
        {
            throw new NotImplementedException();
        }

        protected override Task<string> GetOriginalUrlAsync(IElement textHTML)
        {
            throw new NotImplementedException();
        }

        protected override Task<string> GetRemoteIdAsync(IElement textHTML)
        {
            throw new NotImplementedException();
        }

        protected override Task<string> GetTitleAsync(IElement textHTML)
        {
            throw new NotImplementedException();
        }
    }
}
