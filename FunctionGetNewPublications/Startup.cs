using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PressCenter.Services.Data;

[assembly: FunctionsStartup(typeof(FunctionGetNewPublications.Startup))]
namespace FunctionGetNewPublications
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<FunctionContext>(options => options.UseSqlServer(@"Data Source=tcp:noticiasenoriginal.database.windows.net,1433;Initial Catalog=NoticiasEnOriginal;User Id=antonjekov@noticiasenoriginal.database.windows.net;Password=K13antonj@;"));

            builder.Services.AddTransient<INewsService, NewsService>();
            builder.Services.AddTransient<ITopNewsService, TopNewsService>();
            builder.Services.AddTransient<ISourceService, SourceService>();
        }
    }
}
