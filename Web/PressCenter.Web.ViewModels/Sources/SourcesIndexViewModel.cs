namespace PressCenter.Web.ViewModels.Sources
{
    using System.Collections.Generic;

    public class SourcesIndexViewModel
    {
        public List<int> SelectedSources { get; set; }

        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}
