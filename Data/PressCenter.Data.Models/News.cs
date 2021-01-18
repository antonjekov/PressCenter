﻿namespace PressCenter.Data.Models
{
    using PressCenter.Data.Common.Models;

    public class News : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public int? SourceId { get; set; }

        public virtual Source Source { get; set; }

        public string RemoteId { get; set; }

        public string SearchText { get; set; }
    }
}
