namespace PressCenter.Web.ViewModels.News
{
    using PressCenter.Services.Mapping;
    using PressCenter.Data.Models;
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Slugify;

    public class NewsViewModel : IMapFrom<News>
    {
        private SlugHelper slugHelper;

        public NewsViewModel()
        {
            this.slugHelper = new SlugHelper();
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public int? SourceId { get; set; }

        public virtual Source Source { get; set; }

        public string RemoteId { get; set; }

        public DateTime Date { get; set; }

        public string ShortDate => this.Date.ToString("D", new CultureInfo("es-ES"));

        public string ShortContent => this.GetShortContent(200);

        public string Slug => this.slugHelper.GenerateSlug(this.Title);

        private string GetShortContent(int maxLength)
        {
            if (this.Content.Length <= maxLength)
            {
                return this.Content;
            }
            else
            {
                var content = this.Content.Substring(0, maxLength) + "...";
                return content;
            }
        }
    }
}
