namespace PressCenter.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PressCenter.Data.Models;

    public class SourceSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var sources =
                new
                List<(
                string TypeName,
                string ShortName,
                string Name,
                string Description,
                string EntryPointUrl,
                string Url,
                string DefaultImageUrl,
                bool PageIsDynamic)>
                    {
                         ("PressCenter.Services.Sources.Policia.PoliciaNacional",
                         "Policia Nacional",
                         "Policia Nacional España",
                         " ",
                         "https://www.policia.es/_es/comunicacion_salaprensa.php",
                         "https://www.policia.es/",
                         "/img/policiaNacional.png",
                         true),

                         ("PressCenter.Services.Sources.Policia.GuardiaCivil",
                        "Guardia Civil",
                        "Guardia Civil España",
                        " ",
                        "https://www.guardiacivil.es/es/prensa/noticias/index.html?pagina={0}&buscar=si",
                        "https://www.guardiacivil.es",
                        "/img/guardiaCivil.png",
                        false),

                         ("PressCenter.Services.Sources.Policia.DGT",
                        "DGT",
                        "Dirección General de Tráfico",
                        " ",
                        "https://www.dgt.es/es/prensa/notas-de-prensa/{0}/",
                        "https://www.dgt.es",
                        "/img/dgt.png",
                        false),

                         ("PressCenter.Services.Sources.SpanishGovernment.MinisterioDeSanidad",
                        "Ministerio de Sanidad",
                        "Ministerio de Sanidad España",
                        " ",
                        "https://www.mscbs.gob.es/gabinete/notap_rss.do",
                        "https://www.mscbs.gob.es/",
                        "/img/ministerioDeSanidad.png",
                        false),

                         ("PressCenter.Services.Sources.Catalunya.GeneralitatDeCatalunya",
                         "Generalitat de Catalunya",
                         "Generalitat de Catalunya",
                         " ",
                         "http://web.gencat.cat/es/actualitat/rss.html",
                         "http://web.gencat.cat/",
                         "/img/generalitatDeCatalunya.png",
                         false),
                    };

            foreach (var (typeName, shortName, name, description, entryPointUrl, url, defaultImageUrl, pageIsDynamic) in sources)
            {
                if (!dbContext.Sources.Any(x => x.EntryPointUrl == entryPointUrl))
                {
                    await dbContext.Sources.AddAsync(
                        new Source
                        {
                            TypeName = typeName,
                            PageIsDynamic = pageIsDynamic,
                            ShortName = shortName,
                            Name = name,
                            Description = description,
                            EntryPointUrl = entryPointUrl,
                            Url = url,
                            DefaultImageUrl = defaultImageUrl,
                        });
                }
            }
        }
    }
}
