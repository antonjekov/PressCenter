using CodeHollow.FeedReader;
using PressCenter.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PressCenter.Services.RssAtom
{
    public interface IRssAtomService
    {
        Task<IEnumerable<T>> ReadAsync<T>(string url) where T : INews, new();

        Task<IEnumerable<FeedItem>> Read1Async(string url);
    }
}