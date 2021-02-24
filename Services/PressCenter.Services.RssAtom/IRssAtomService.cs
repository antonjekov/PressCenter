using CodeHollow.FeedReader;
using PressCenter.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PressCenter.Services.RssAtom
{
    public interface IRssAtomService
    {
        Task<IEnumerable<FeedItem>> ReadAsync(string url);
    }
}