namespace PressCenter.Web.ViewModels.News
{
    using System.Collections.Generic;

    using PressCenter.Web.ViewModels.Shared;

    public class NewsViewModelPagination : PaginationViewModel
    {
        public IEnumerable<NewsViewModel> News { get; set; }

        public int ItemsTodayCount { get; set; }
    }
}
