namespace PressCenter.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PressCenter.Data.Models;

    public class TopNewsSourceSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var sources =
                new
                List<(string TypeName, string Name, string Url)>
                    {
                        ("PressCenter.Services.Sources.Medias.Marca", "marca.com", "https://www.marca.com/"),
                        ("PressCenter.Services.Sources.Medias.ElPais", "elpais.com", "https://elpais.com/"),
                    };

            foreach (var source in sources)
            {
                if (!dbContext.TopNewsSources.Any(x => x.Url == source.Url))
                {
                    await dbContext.TopNewsSources.AddAsync(
                        new TopNewsSource
                        {
                            TypeName = source.TypeName,
                            Name = source.Name,
                            Url = source.Url,
                        });
                }
            }
        }
    }
}
