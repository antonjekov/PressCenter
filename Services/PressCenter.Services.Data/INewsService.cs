namespace PressCenter.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PressCenter.Data.Models;
    using PressCenter.Services.Sources;

    public interface INewsService
    {
        Task AddAsync(RemoteNews remoteNews, int sourceId);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        IEnumerable<News> GetAll();

        List<string> GetAllRemoteIds();

        List<string> GetAllRemoteIdsBySourceId(int id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        int Count();

        int NewsTodayCount();

        Task DeleteAsync(int id);
    }
}
