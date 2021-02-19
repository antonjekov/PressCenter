using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using PressCenter.Data.Models;
using PressCenter.Services.RssAtom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.Medias
{
    public class Marca : BaseTopNewsSource<IElement>
    {
        private readonly IBrowsingContext context;

        public Marca(TopNewsSource source, IRssAtomService rssAtomService) : base(source, rssAtomService)
        {
            this.context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        }
        public override async Task<IEnumerable<TopNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<TopNews>();
            try
            {
                var page = await this.context.OpenAsync(this.Url);
                var portada = page.QuerySelector("#portada");
                var sections = portada.QuerySelectorAll("section");
                var topNews = sections[1].QuerySelector("li");
                TopNews input = await GetInfoAsync(topNews);
                if (existingNewsRemoteIds.Contains(input.RemoteId))
                {
                    return result;
                }
                else
                {
                    result.Add(input);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override async Task<string> GetImageUrlAsync(IElement textHTML)
        {
            var figure = textHTML.QuerySelector("figure");
            string imgUrl = String.Empty;
            if (figure != null)
            {
                var img = figure.QuerySelector("img");
                imgUrl = img.GetAttribute("src");
            }
            return imgUrl;
        }

        protected override async Task<string> GetOriginalUrlAsync(IElement textHTML)
        {
            var figure = textHTML.QuerySelector("figure");
            var url = figure.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
            return url;
        }

        protected override async Task<string> GetRemoteIdAsync(IElement textHTML)
        {
            var url = await this.GetOriginalUrlAsync(textHTML);
            var idInfo = url.Split(".html")[0];
            var id = idInfo[^24..];
            return id;
        }

        protected override async Task<string> GetTitleAsync(IElement textHTML)
        {
            var titleElement = textHTML.QuerySelector("h2");
            var title = titleElement.TextContent;
            return title;
        }
    }
}
