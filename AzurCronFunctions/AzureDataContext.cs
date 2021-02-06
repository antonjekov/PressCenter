using Microsoft.EntityFrameworkCore;
using PressCenter.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzurCronFunctions
{
    public class AzureDataContext: DbContext
    {
        public AzureDataContext(DbContextOptions<AzureDataContext> options) : base(options)
        {
        }
        //public DbSet<TopNews> TopNews { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Source> Sources { get; set; }
    }
}
