using PressCenter.Data.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Sources.Medias
{
    public class ElPais : BaseTopNewsSource
    {
        public ElPais(TopNewsSource source) : base(source)
        {
        }
        public override async Task<IEnumerable<TopNews>> GetAllPublicationsAsync()
        {
            var result = new List<TopNews>();
            //await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            //var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //{
            //    Headless = true
            //});
            var options = new ConnectOptions()
            {
                BrowserWSEndpoint = $"wss://chrome.browserless.io/"
            };
            var browser = await Puppeteer.ConnectAsync(options);
            try
            {
                var page = await browser.NewPageAsync();
                var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36";
                await page.SetUserAgentAsync(userAgent);
                await page.GoToAsync(this.Url);
                var topNews = await page.QuerySelectorAsync(".first_column");
                TopNews input = await GetInfoAsync(topNews);
                result.Add(input);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (browser != null)
                {
                    await browser.CloseAsync();
                }
            }
        }

        public override async Task<IEnumerable<TopNews>> GetNewPublicationsAsync(List<string> existingNewsRemoteIds)
        {
            var result = new List<TopNews>();
            //await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            //var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //{
            //    Headless = true
            //});
            var options = new ConnectOptions()
            {
                BrowserWSEndpoint = $"wss://chrome.browserless.io/"
            };
            var browser = await Puppeteer.ConnectAsync(options);

            try
            {
                var page = await browser.NewPageAsync();
                var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36";
                await page.SetUserAgentAsync(userAgent);
                await page.GoToAsync(this.Url);
                var topNews = await page.QuerySelectorAsync(".first_column");
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
            finally
            {
                if (browser != null)
                {
                    await browser.CloseAsync();
                }
            }
        }

        protected override async Task<string> GetImageUrlAsync(ElementHandle textHTML)
        {
            var img = await textHTML.QuerySelectorAsync("img");
            if (img!=null)
            {
                var imgUrlInfo = await img.GetPropertyAsync("src");
                var imgUrl = await imgUrlInfo.JsonValueAsync<string>();
                return imgUrl;
            }
            
            return String.Empty;
        }

        protected override async Task<string> GetOriginalUrlAsync(ElementHandle textHTML)
        {
            var titleElement = await textHTML.QuerySelectorAsync("h2");
            var a = await titleElement.QuerySelectorAsync("a");
            var imgUrlInfo = await a.GetPropertyAsync("href");
            var url = await imgUrlInfo.JsonValueAsync<string>();
            return url;
        }

        protected override async Task<string> GetRemoteIdAsync(ElementHandle textHTML)
        {
            return await this.GetTitleAsync(textHTML);
        }

        protected override async Task<string> GetTitleAsync(ElementHandle textHTML)
        {
            var titleElement = await textHTML.QuerySelectorAsync("h2");
            var titleInfo = await titleElement.GetPropertyAsync("innerText");
            var title = await titleInfo.JsonValueAsync<string>();
            return title;
        }
    }
}
