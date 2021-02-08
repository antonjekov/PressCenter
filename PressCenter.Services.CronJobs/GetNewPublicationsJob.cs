using AngleSharp.Dom;
//using Hangfire;
using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using PressCenter.Services.Data;
using PressCenter.Services.Sources;
//using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressCenter.Services.CronJobs
{
    public class GetNewPublicationsJob : IGetNewPublicationsJob
    {
        private readonly INewsService newsService;

        public GetNewPublicationsJob(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public async Task StartAsync(Source source)
        {
            var type = typeof(BaseSource<IElement>).Assembly.GetType(source.TypeName);
            //if (source.ShortName == "Policia Nacional")
            //{
            //    type = typeof(BaseSource<ElementHandle>).Assembly.GetType(source.TypeName);
            //}
            if (type == null)
            {
                throw new Exception($"Type \"{source.TypeName}\" not found!");
            }

            //if (source.ShortName == "Policia Nacional")
            //{
            //    var instance = (BaseSource<ElementHandle>)Activator.CreateInstance(type, source);
            //    if (instance == null)
            //    {
            //        throw new Exception($"Unable to create {typeof(BaseSource<ElementHandle>).Name} instance from \"{source.TypeName}\"!");
            //    }
            //    var newPublications = await instance.GetNewPublicationsAsync(this.resourceIds);
            //    foreach (var publication in newPublications)
            //    {
            //        await this.newsService.AddAsync(publication, source.Id);
            //    }
            //}
            //else
            //{
            //    var instance = (BaseSource<IElement>)Activator.CreateInstance(type, source);
            //    if (instance == null)
            //    {
            //        throw new Exception($"Unable to create {typeof(BaseSource<IElement>).Name} instance from \"{source.TypeName}\"!");
            //    }
            //    var newPublications = await instance.GetNewPublicationsAsync(this.resourceIds);
            //    foreach (var publication in newPublications)
            //    {
            //        await this.newsService.AddAsync(publication, source.Id);
            //    }
            //}
            var instance = (BaseSource<IElement>)Activator.CreateInstance(type, source);
            if (instance == null)
            {
                throw new Exception($"Unable to create {typeof(BaseSource<IElement>).Name} instance from \"{source.TypeName}\"!");
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
