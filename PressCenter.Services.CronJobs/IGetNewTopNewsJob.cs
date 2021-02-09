using PressCenter.Data.Models;
using System.Threading.Tasks;

namespace PressCenter.Services.CronJobs
{
    public interface IGetNewTopNewsJob
    {
        Task StartAsync(TopNewsSource source);
    }
}