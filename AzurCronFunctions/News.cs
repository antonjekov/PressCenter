using System;
using System.Collections.Generic;
using System.Text;

namespace AzurCronFunctions
{
    public class News
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }

        public string OriginalUrl { get; set; }

        public int? SourceId { get; set; }

        public string RemoteId { get; set; }

        public string SearchText { get; set; }
    }
}
