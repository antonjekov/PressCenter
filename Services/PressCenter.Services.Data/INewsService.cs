namespace PressCenter.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PressCenter.Services.Sources;

    public interface INewsService
    {
        Task AddAsync(RemoteNews remoteNews, int sourceId);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        int Count();

        int NewsTodayCount();
    }
}
