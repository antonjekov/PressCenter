namespace PressCenter.Web.ViewModels.News
{
    using PressCenter.Services.Mapping;
    using PressCenter.Data.Models;
    using System;
    using System.Globalization;

    public class NewsViewModel : IMapFrom<News>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public int? SourceId { get; set; }

        public virtual Source Source { get; set; }

        public string RemoteId { get; set; }

        public DateTime Date { get; set; }

        public string ShortDate => this.Date.ToString(new CultureInfo("es-ES").DateTimeFormat.LongDatePattern);
    }
}
