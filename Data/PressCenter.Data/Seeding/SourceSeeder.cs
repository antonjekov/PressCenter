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
                string DefaultImageUrl)>
                    {
                        //("PressCenter.Services.Sources.SpanishGovernment.MinisterioInteriorSource",
                        //"Ministerio del Interior",
                        //"Ministerio del Interior - Gobierno de España",
                        //" ",
                        //"http://www.interior.gob.es/es/prensa/noticias?p_p_id=101_INSTANCE_GHU8Ap6ztgsg&p_p_lifecycle=0&p_p_state=normal&p_p_mode=view&p_p_col_id=column-2&p_p_col_count=1&_101_INSTANCE_GHU8Ap6ztgsg_delta=10&_101_INSTANCE_GHU8Ap6ztgsg_keywords=&_101_INSTANCE_GHU8Ap6ztgsg_advancedSearch=false&_101_INSTANCE_GHU8Ap6ztgsg_andOperator=true&p_r_p_564233524_resetCur=false&_101_INSTANCE_GHU8Ap6ztgsg_cur=",
                        //"http://www.interior.gob.es/",
                        //"/img/ministerioInterior.png"),
                        ("PressCenter.Services.Sources.Policia.PoliciaNacional",
                        "Policia Nacional",
                        "Policia Nacional España",
                        " ",
                        "https://www.policia.es/prensa/historico/indice.php?y={0}&_pagi_pg={1}",
                        "https://www.policia.es/",
                        "/img/policiaNacional.png"),
                        ("PressCenter.Services.Sources.Policia.GuardiaCivil",
                        "Guardia Civil",
                        "Guardia Civil España",
                        " ",
                        "https://www.guardiacivil.es/es/prensa/noticias/index.html?pagina={0}&buscar=si",
                        "https://www.guardiacivil.es",
                        "/img/guardiaCivil.png"),
                        ("PressCenter.Services.Sources.Policia.DGT",
                        "DGT",
                        "Dirección General de Tráfico",
                        " ",
                        "https://www.dgt.es/es/prensa/notas-de-prensa/{0}/",
                        "https://www.dgt.es",
                        "/img/dgt.png"),
                    };

            foreach (var source in sources)
            {
                if (!dbContext.Sources.Any(x => x.EntryPointUrl == source.EntryPointUrl))
                {
                    await dbContext.Sources.AddAsync(
                        new Source
                        {
                            TypeName = source.TypeName,
                            ShortName = source.ShortName,
                            Name = source.Name,
                            Description = source.Description,
                            EntryPointUrl = source.EntryPointUrl,
                            Url = source.Url,
                            DefaultImageUrl = source.DefaultImageUrl,
                        });
                }
            }
        }
    }
}
