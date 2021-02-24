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
        public async Task<IEnumerable<FeedItem>> ReadAsync(string url) 
        {
            var feed = await FeedReader.ReadAsync(url);
            var items = feed.Items;            
            return items;
        }
    }
}
