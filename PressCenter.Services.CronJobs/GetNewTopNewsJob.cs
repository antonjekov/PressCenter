using PressCenter.Data.Models;
using PressCenter.Services.Data;
using PressCenter.Services.Sources;
using System;
using System.Threading.Tasks;

namespace PressCenter.Services.CronJobs
{
    public class GetNewTopNewsJob : IGetNewTopNewsJob
    {
        private readonly ITopNewsService newsService;

        public GetNewTopNewsJob(ITopNewsService newsService)
        {
            this.newsService = newsService;
        }

        public async Task StartAsync(TopNewsSource source)
        {
            var type = typeof(BaseTopNewsSource).Assembly.GetType(source.TypeName);
            if (type == null)
            {
                throw new Exception($"Type \"{source.TypeName}\" not found!");
            }

            var instance = (BaseTopNewsSource)Activator.CreateInstance(type, source);
            if (instance == null)
            {
                throw new Exception($"Unable to create {typeof(BaseTopNewsSource).Name} instance from \"{source.TypeName}\"!");
            }

            var resourceIds = this.newsService.GetAllRemoteIds();
            var newPublications = await instance.GetNewPublicationsAsync(resourceIds);
            foreach (var publication in newPublications)
            {
                await this.newsService.AddAsync(publication, source.Id);
            }
        }
    }
}
