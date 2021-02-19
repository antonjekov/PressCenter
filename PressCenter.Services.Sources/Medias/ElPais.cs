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
    public class ElPais : BaseTopNewsSource<IElement>
    {
        private readonly IBrowsingContext context;

        public ElPais(TopNewsSource source, IRssAtomService rssAtomService) : base(source, rssAtomService)
        {
            this.context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        }
        public override async Task<IEnumerable<TopNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<TopNews>();
            try
            {
                var page = await this.context.OpenAsync(this.Url);
                var topNews = page.QuerySelector(".second_column");
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
            var img = textHTML.QuerySelector("img");
            if (img!=null)
            {
                var imgUrl = img.GetAttribute("src");
                return imgUrl;
            }
            
            return String.Empty;
        }

        protected override async Task<string> GetOriginalUrlAsync(IElement textHTML)
        {
            var titleElement = textHTML.QuerySelector("h2");
            var url = titleElement.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().FirstOrDefault().Href;
            return url;
        }

        protected override async Task<string> GetRemoteIdAsync(IElement textHTML)
        {
            return await this.GetTitleAsync(textHTML);
        }

        protected override async Task<string> GetTitleAsync(IElement textHTML)
        {
            var titleElement = textHTML.QuerySelector("h2");
            var title = titleElement.TextContent;
            return title;
        }
    }
}
