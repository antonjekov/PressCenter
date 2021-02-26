namespace PressCenter.Data.Models
{
    using System;

    using PressCenter.Data.Common.Models;

    public class News : BaseDeletableModel<int>, INews
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }

        public string OriginalUrl { get; set; }

        public int SourceId { get; set; }

        public virtual Source Source { get; set; }

        public string RemoteId { get; set; }
    }
}
