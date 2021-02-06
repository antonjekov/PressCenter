using Microsoft.EntityFrameworkCore;
using PressCenter.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionGetNewPublications
{
    public class FunctionContext : DbContext
    {
        public FunctionContext(DbContextOptions<FunctionContext> options) : base(options)
        {

        }

        public DbSet<Source> Sources { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<TopNews> TopNews { get; set; }

        public DbSet<TopNewsSource> TopNewsSources { get; set; }
    }
}
