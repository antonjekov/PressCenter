namespace PressCenter.Services.Data
{
    using System.Collections.Generic;

    using PressCenter.Data.Models;

    public interface ITopNewsSourceService
    {
        IEnumerable<TopNewsSource> GetAll();

        List<string> GetAllNewsRemoteIdsForSource(int sourceId);
    }
}
