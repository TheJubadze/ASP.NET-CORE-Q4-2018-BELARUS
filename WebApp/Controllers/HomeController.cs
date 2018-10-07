using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private const string WelcomeMsg = "Welcome!";

        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            return View((object)WelcomeMsg);
        }
    }
}
