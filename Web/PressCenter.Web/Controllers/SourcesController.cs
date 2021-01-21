using Microsoft.AspNetCore.Mvc;
using PressCenter.Services.Data;
using PressCenter.Web.ViewModels.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var sources = this.sourceService.GetAll<SourceViewModel>();
            return this.View(sources);
        }
    }
}
