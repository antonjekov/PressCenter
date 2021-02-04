namespace PressCenter.Data.Models
{
    public interface ITopNews
    {
        string ImageUrl { get; set; }

        string OriginalUrl { get; set; }

        string RemoteId { get; set; }

        string Title { get; set; }
    }
}