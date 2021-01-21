using PressCenter.Data.Models;
using System.Collections.Generic;

namespace PressCenter.Services.Data
{
    public interface ISourceService
    {
        IEnumerable<Source> GetAll();

        IEnumerable<T> GetAll<T>();
    }
}
