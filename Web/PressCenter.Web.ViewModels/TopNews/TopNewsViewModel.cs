namespace PressCenter.Web.ViewModels.TopNews
{
    using System;
    using System.Globalization;

    using PressCenter.Data.Models;
    using PressCenter.Services.Mapping;

    public class TopNewsViewModel : IMapFrom<TopNews>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public int SourceId { get; set; }

        public virtual TopNewsSource Source { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ShortDate => this.CreatedOn.ToString("D", new CultureInfo("es-ES"));
    }
}
