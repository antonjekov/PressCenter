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
                List<(string TypeName, string Name, string Url, bool PageIsDynamic)>
                    {
                        ("PressCenter.Services.Sources.Medias.Marca", "marca.com", "https://www.marca.com/", false),
                        ("PressCenter.Services.Sources.Medias.ElPais", "elpais.com", "https://elpais.com/", false),
                        ("PressCenter.Services.Sources.Medias.LaVanguardia", "lavanguardia.com", "https://www.lavanguardia.com/newsml/home.xml", false),
                        ("PressCenter.Services.Sources.Medias.ElMundo", "elmundo.com", "https://e00-elmundo.uecdn.es/elmundo/rss/portada.xml", false),
                    };

            foreach (var (typeName, name, url, pageIsDynamic) in sources)
            {
                if (!dbContext.TopNewsSources.Any(x => x.Url == url))
                {
                    await dbContext.TopNewsSources.AddAsync(
                        new TopNewsSource
                        {
                            TypeName = typeName,
                            Name = name,
                            Url = url,
                            PageIsDynamic = pageIsDynamic,
                        });
                }
            }
        }
    }
}
