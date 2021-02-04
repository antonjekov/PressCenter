using PressCenter.Web.ViewModels.TopNews;
using System;
using System.Collections.Generic;
using System.Text;

namespace PressCenter.Web.ViewModels.News
{
    public class NewsDetailsViewModel
    {
        public IEnumerable<TopNewsViewModel> TopNews { get; set; }

        public NewsViewModel News { get; set; }
    }
}
