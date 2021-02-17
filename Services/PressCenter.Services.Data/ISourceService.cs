namespace PressCenter.Services.Data
{
    using System.Collections.Generic;

    using PressCenter.Data.Models;

    public interface ISourceService
    {
        IEnumerable<Source> GetAll();

        IEnumerable<T> GetAll<T>();

        List<string> GetAllNewsRemoteIdsForSource(int sourceId);
    }
}
