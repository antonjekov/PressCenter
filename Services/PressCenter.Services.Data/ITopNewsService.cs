using PressCenter.Services.Sources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Data
{
    public interface ITopNewsService
    {
        Task AddAsync(TopNews topNews, int sourceId);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        int Count();
    }
}
