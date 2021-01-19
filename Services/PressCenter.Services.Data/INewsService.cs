namespace PressCenter.Services.Data
{
    using System.Threading.Tasks;

    using PressCenter.Services.Sources;

    public interface INewsService
    {
        Task AddAsync(RemoteNews remoteNews, int sourceId);

        int Count();
    }
}
