﻿namespace PressCenter.Web
{
    using System;
    using System.Reflection;

    using Hangfire;
    using Hangfire.Console;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PressCenter.Data;
    using PressCenter.Data.Common;
    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;
    using PressCenter.Data.Repositories;
    using PressCenter.Data.Seeding;
    using PressCenter.Services.Data;
    using PressCenter.Services.Mapping;
    using PressCenter.Services.Messaging;
    using PressCenter.Services.RssAtom;
    using PressCenter.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            // services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<ITopNewsService, TopNewsService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<IRssAtomService, RssAtomService>();
            services.AddTransient<IDataValidationService, DataValidationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("news", "News/{id:int:min(1)}/{slug?}", new { controller = "News", action = "Details", });
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
