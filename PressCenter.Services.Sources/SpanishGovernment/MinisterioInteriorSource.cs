

namespace PressCenter.Services.Sources.SpanishGovernment
{
    using System.Collections.Generic;
    using AngleSharp;
    using PressCenter.Services;
    public class MinisterioInteriorSource : BaseSource
    {
        private string entryPointUrl;

        public MinisterioInteriorSource(string entryPointUrl)
        {
            this.entryPointUrl = entryPointUrl;
        }
        public override string EntryPointUrl => entryPointUrl;

        public override async System.Threading.Tasks.Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync()
        {
            var result = new List<RemoteNews>();
            for (int i = 0; i < 10; i++)
            {
                var url = this.entryPointUrl + i;
                var document = await this.Context.OpenAsync(url);
                var elementas = document.QuerySelectorAll("div.noticias");
            }
            return result;
        }

        public override IEnumerable<RemoteNews> GetLatestPublications()
        {
            throw new System.NotImplementedException();
        }
    }
}
