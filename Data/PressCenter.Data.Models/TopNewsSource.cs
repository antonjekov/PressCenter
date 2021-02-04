using PressCenter.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PressCenter.Data.Models
{
    public class TopNewsSource: BaseDeletableModel<int>
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
