using PressCenter.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PressCenter.Services.Sources
{
    public class TopNews : ITopNews
    {
        public TopNews(string title, string imageUrl, string originalUrl, string remoteId)
        {
            this.Title = title;
            this.ImageUrl = imageUrl;
            this.OriginalUrl = originalUrl;
            this.RemoteId = remoteId;
        }
        public string ImageUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string RemoteId { get; set; }
        public string Title { get; set; }
    }
}
