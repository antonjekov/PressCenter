using AzurCronFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PressCenter.Data.Common;
using PressCenter.Data.Common.Repositories;
using PressCenter.Services.Data;
using System;

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
            builder.Services.AddDbContext<AzureDataContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));

            builder.Services.AddTransient<INewsService, NewsService>();
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
