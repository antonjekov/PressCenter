namespace PressCenter.Web.ViewModels.Sources
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using PressCenter.Data.Models;
    using PressCenter.Services.Mapping;

    public class SourceViewModel : IMapFrom<Source>
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public string ShortName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string EntryPointUrl { get; set; }

        public string Url { get; set; }

        public string DefaultImageUrl { get; set; }

        public ICollection<News> News { get; set; }

        public int NewsCount => this.News.Count;
    }
}
