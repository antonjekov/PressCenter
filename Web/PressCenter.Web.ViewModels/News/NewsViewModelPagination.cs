namespace PressCenter.Web.ViewModels.News
{
    using System.Collections.Generic;

    using PressCenter.Web.ViewModels.Shared;
    using PressCenter.Web.ViewModels.TopNews;

    public class NewsViewModelPagination : PaginationViewModel
    {
        public IEnumerable<NewsViewModel> News { get; set; }

        public IEnumerable<TopNewsViewModel> TopNews { get; set; }

        public int ItemsTodayCount { get; set; }
    }
}
