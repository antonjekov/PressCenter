using PressCenter.Data.Models;
using System.Threading.Tasks;

namespace PressCenter.Services.CronJobs
{
    public interface IGetNewPublicationsJob
    {
        Task StartAsync(Source source);
    }
}