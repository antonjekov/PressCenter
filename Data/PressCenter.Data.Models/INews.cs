namespace PressCenter.Data.Models
{
    using System;

    public interface INews
    {
        string Content { get; set; }

        DateTime Date { get; set; }

        string ImageUrl { get; set; }

        string OriginalUrl { get; set; }

        string RemoteId { get; set; }

        string Title { get; set; }
    }
}
