using Microsoft.AspNetCore.Mvc;
using PressCenter.Services.Data;
using PressCenter.Web.ViewModels.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PressCenter.Web.Controllers
{
    public class SourcesController : BaseController
    {
        private ISourceService sourceService;

        public SourcesController(ISourceService sourceService)
        {
            this.sourceService = sourceService;
        }

        public IActionResult Index()
        {
            var sources = this.sourceService.GetAll<SourceViewModel>().OrderBy(x => x.ShortName);
            var selectedSourcesJson = this.Request.Cookies["sourceSelect"];
            List<int> selectedSources = new List<int>();
            if (selectedSourcesJson != null)
            {
                selectedSources = JsonSerializer.Deserialize<List<int>>(selectedSourcesJson);                
            }

            var allSources = new SourcesIndexViewModel()
            {
                Sources = sources,
                SelectedSources = selectedSources,
            };
            return this.View(allSources);
        }
    }
}
