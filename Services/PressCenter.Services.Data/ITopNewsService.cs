namespace PressCenter.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PressCenter.Services.Sources;

    public interface ITopNewsService
    {
        Task AddAsync(TopNews topNews, int sourceId);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllFromToday<T>();

        List<string> GetAllRemoteIds();

        List<string> GetAllRemoteIdsBySourceId(int id);

        int Count();
    }
}
