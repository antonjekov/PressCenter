using Microsoft.AspNetCore.Mvc;

namespace PressCenter.Web.Controllers
{
    public class ContactsController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
