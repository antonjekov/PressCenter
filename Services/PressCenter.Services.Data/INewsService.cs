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

        IEnumerable<T> GetLastN<T>(int n);

        IEnumerable<News> GetAll();

        List<string> GetAllRemoteIds();

        List<string> GetAllRemoteIdsBySourceId(int id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage, List<int> sourceIds);

        int Count();

        int Count(List<int> sourceIds);

        int NewsTodayCount();

        int NewsTodayCount(List<int> sourceIds);

        Task DeleteAsync(int id);
    }
}
