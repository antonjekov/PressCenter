using CodeHollow.FeedReader;
using PressCenter.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.RssAtom
{
    public class RssAtomService : IRssAtomService
    {
        public async Task<IEnumerable<T>> ReadAsync<T>(string url) where T : INews, new()
        {
            var feed = await FeedReader.ReadAsync(url);
            var items = feed.Items
                .Select(x => new T()
                {
                    Title = x.Title,
                    Content = x.Content,
                    Date = (DateTime)x.PublishingDate,
                    OriginalUrl = x.Link
                }).ToList();
            return items;
        }

        public async Task<IEnumerable<FeedItem>> Read1Async(string url) 
        {
            var feed = await FeedReader.ReadAsync(url);
            var items = feed.Items;            
            return items;
        }
    }
}
