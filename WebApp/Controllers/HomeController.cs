using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private const string WelcomeMsg = "Welcome!";

        [DefaultBreadcrumb("Home")]
        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            return View((object)WelcomeMsg);
        }
    }
}
