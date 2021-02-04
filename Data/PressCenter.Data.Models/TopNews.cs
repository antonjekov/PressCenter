namespace PressCenter.Data.Models
{
    using PressCenter.Data.Common.Models;

    public class TopNews : BaseDeletableModel<int>, ITopNews
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string OriginalUrl { get; set; }

        public int SourceId { get; set; }

        public virtual TopNewsSource Source { get; set; }

        public string RemoteId { get; set; }
    }
}
