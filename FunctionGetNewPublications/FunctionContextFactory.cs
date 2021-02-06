using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionGetNewPublications
{
    public class FunctionContextFactory : IDesignTimeDbContextFactory<FunctionContext>
    {
        public FunctionContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FunctionContext>();
            optionsBuilder.UseSqlServer(@"Data Source=tcp:noticiasenoriginal.database.windows.net,1433;Initial Catalog=NoticiasEnOriginal;User Id=antonjekov@noticiasenoriginal.database.windows.net;Password=K13antonj@;");
            return new FunctionContext(optionsBuilder.Options);
        }
    }
}
