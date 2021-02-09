using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using PressCenter.Data.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.Policia
{
    public class PoliciaNacional : BaseSource<ElementHandle>
    {
        public PoliciaNacional(Source source) : base(source)
        {
        }

        public override async Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
        {
            var result = new List<RemoteNews>();
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36";
            await page.SetUserAgentAsync(userAgent);
            await page.GoToAsync(this.EntryPointUrl);
            await page.ClickAsync("#aceptaCookies");
            await page.WaitForNavigationAsync();
            var noticeiro = await page.QuerySelectorAsync("#div_noticiero");
            var noticias = await noticeiro.QuerySelectorAllAsync("li");
            foreach (var item in noticias)
            {
                RemoteNews input = await GetInfoAsync(item);
                result.Add(input);
            }
            await browser.CloseAsync();
            return result;
        }

        public override async Task<IEnumerable<RemoteNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<RemoteNews>();
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            }) ;
            var page = await browser.NewPageAsync();
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36";
            await page.SetUserAgentAsync(userAgent);
            await page.GoToAsync(this.EntryPointUrl);
            await page.ClickAsync("#aceptaCookies");
            var noticeiro = await page.QuerySelectorAsync("#div_noticiero");
            var noticias = await noticeiro.QuerySelectorAllAsync("li");
            foreach (var item in noticias)
            {
                RemoteNews input = await GetInfoAsync(item);
                if (existingNewsRemoteIds.Contains(input.RemoteId))
                {
                    break;
                }
                result.Add(input);
            }
            await browser.CloseAsync();
            return result;
        }

        protected override async Task<DateTime> GetDateAsync(ElementHandle textHTML)
        {
            var elements = await textHTML.QuerySelectorAllAsync("p");
            var dataInfo = await elements[^1].GetPropertyAsync("innerText");
            var dataString = await dataInfo.JsonValueAsync<string>();
            var data = DateTime.ParseExact(dataString, "dd/MM/yy", CultureInfo.InvariantCulture);
            return data;
        }

        protected override async Task<string> GetImageUrlAsync(ElementHandle textHTML)
        {
            var img = await textHTML.QuerySelectorAsync("img");
            string imgUrl;
            if (img != null)
            {
                var imgUrlInfo = await img.GetPropertyAsync("src");
                imgUrl = await imgUrlInfo.JsonValueAsync<string>();
            }
            else
            {
                imgUrl = this.DefaultImageUrl;
            }
            return imgUrl;
        }

        protected override async Task<string> GetNewsContentAsync(ElementHandle textHTML)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
            var hrefString = await this.GetOriginalUrlAsync(textHTML);
            var pageDetails = await browser.NewPageAsync();
            await pageDetails.GoToAsync(hrefString);
            var contentInfoDiv = await pageDetails.QuerySelectorAsync(".holdDetalle");
            var contentInfoP = await contentInfoDiv.QuerySelectorAllAsync("p");
            var sb = new StringBuilder();
            for (int i = 0; i < contentInfoP.Length; i++)
            {
                var pInnerText = await contentInfoP[i].GetPropertyAsync("innerText");                
                var pString = await pInnerText.JsonValueAsync<string>();
                sb.AppendLine(pString);
            }
            await browser.CloseAsync();
            return sb.ToString();
        }

        protected override async Task<string> GetOriginalUrlAsync(ElementHandle textHTML)
        {
            var elements = await textHTML.QuerySelectorAllAsync("p");
            var hrefInfo = await elements[0].QuerySelectorAsync("a");
            var href = await hrefInfo.GetPropertyAsync("href");
            var hrefString = await href.JsonValueAsync<string>();
            return hrefString;
        }

        protected override async Task<string> GetRemoteIdAsync(ElementHandle textHTML)
        {
            var hrefString = await this.GetOriginalUrlAsync(textHTML);
            var originalId = hrefString[^4..];
            return originalId;
        }

        protected override async Task<string> GetTitleAsync(ElementHandle textHTML)
        {
            var elements = await textHTML.QuerySelectorAllAsync("p");
            var titleInfo = await elements[0].GetPropertyAsync("innerText");
            var title = await titleInfo.JsonValueAsync<string>();
            return title;
        }
    }
}
