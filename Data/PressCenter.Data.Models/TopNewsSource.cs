namespace PressCenter.Data.Models
{
    using System.Collections.Generic;

    using PressCenter.Data.Common.Models;

    public class TopNewsSource : BaseDeletableModel<int>
    {
        public TopNewsSource()
        {
            this.TopNews = new HashSet<TopNews>();
        }

        public string TypeName { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public virtual ICollection<TopNews> TopNews { get; set; }
    }
}
