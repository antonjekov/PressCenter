using System;
using System.Collections.Generic;
using System.Text;

namespace PressCenter.Services.Sources
{
    public class RemoteNews : INews
    {
        public RemoteNews(string title, string content, DateTime date, string imageUrl, string remoteId, string originalUrl)
        {
            this.Title = title;
            this.Content = content;
            this.Date = date;
            this.ImageUrl = imageUrl;
            this.RemoteId = remoteId;
            this.OriginalUrl = originalUrl;
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public DateTime Date { get; set; }

        public string RemoteId { get; set; }
    }
}
