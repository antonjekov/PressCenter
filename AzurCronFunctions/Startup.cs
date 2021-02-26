using AzurCronFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PressCenter.Data;
using PressCenter.Data.Common;
using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Repositories;
using PressCenter.Services.CronJobs;
using PressCenter.Services.Data;
using PressCenter.Services.Mapping;
using PressCenter.Services.RssAtom;
using PressCenter.Web.ViewModels;
using System;
using System.Linq;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzurCronFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (connectionString==null)
            {
                connectionString = GetSqlAzureConnectionString("SqlConnectionString");
            }
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));

            // Data repositories
            builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            builder.Services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            builder.Services.AddTransient<INewsService, NewsService>();
            builder.Services.AddTransient<ITopNewsService, TopNewsService>();
            builder.Services.AddTransient<ISourceService, SourceService>();
            builder.Services.AddTransient<ITopNewsSourceService, TopNewsSourceService>();
            builder.Services.AddTransient<IGetNewPublicationsJob, GetNewPublicationsJob>();
            builder.Services.AddTransient<IGetNewTopNewsJob, GetNewTopNewsJob>();
            builder.Services.AddTransient<IRssAtomService, RssAtomService>();
            builder.Services.AddTransient<IDataValidationService, DataValidationService>();
            
            // Set AutoMapper
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        public static string GetSqlAzureConnectionString(string name)
        {
            string conStr = System.Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(conStr)) // Azure Functions App Service naming convention
                conStr = System.Environment.GetEnvironmentVariable($"SQLAZURECONNSTR_{name}", EnvironmentVariableTarget.Process);
            return conStr;
        }

        
    }
}
