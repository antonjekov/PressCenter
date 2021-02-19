using AngleSharp.Dom;
using PressCenter.Data.Models;
using PressCenter.Services.Data;
using PressCenter.Services.RssAtom;
using PressCenter.Services.Sources;
using PuppeteerSharp;
using System;
using System.Threading.Tasks;

namespace PressCenter.Services.CronJobs
{
    public class GetNewTopNewsJob : IGetNewTopNewsJob
    {
        private readonly ITopNewsService newsService;
        private readonly IRssAtomService rssAtomService;

        public GetNewTopNewsJob(ITopNewsService newsService, IRssAtomService rssAtomService)
        {
            this.newsService = newsService;
            this.rssAtomService = rssAtomService;
        }

        public async Task StartAsync(TopNewsSource source)
        {
            var type = typeof(BaseTopNewsSource<IElement>).Assembly.GetType(source.TypeName);
            if (source.PageIsDynamic)
            {
                type = typeof(BaseTopNewsSource<ElementHandle>).Assembly.GetType(source.TypeName);
            }

            if (type == null)
            {
                throw new Exception($"Type \"{source.TypeName}\" not found!");
            }

            if (source.PageIsDynamic)
            {
                var instance = (BaseTopNewsSource<ElementHandle>)Activator.CreateInstance(type, source, rssAtomService);
                if (instance == null)
                {
                    throw new Exception($"Unable to create {typeof(BaseTopNewsSource<ElementHandle>).Name} instance from \"{source.TypeName}\"!");
                }

                var resourceIds = this.newsService.GetAllRemoteIdsBySourceId(source.Id); 
                var newPublications = await instance.GetNewPublicationsAsync(resourceIds);
                foreach (var publication in newPublications)
                {
                    await this.newsService.AddAsync(publication, source.Id);
                }
            }
            else
            {
                var instance = (BaseTopNewsSource<IElement>)Activator.CreateInstance(type, source, rssAtomService);
                if (instance == null)
                {
                    throw new Exception($"Unable to create {typeof(BaseTopNewsSource<IElement>).Name} instance from \"{source.TypeName}\"!");
                }

                var resourceIds = this.newsService.GetAllRemoteIdsBySourceId(source.Id);
                var newPublications = await instance.GetNewPublicationsAsync(resourceIds);
                foreach (var publication in newPublications)
                {
                    await this.newsService.AddAsync(publication, source.Id);
                }
            }            
        }
    }
}
