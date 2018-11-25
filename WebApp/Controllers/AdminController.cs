using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Common;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = Constants.USER_ROLE_ADMIN)]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Users()
        {
            var model = new UsersViewModel {Users = _userManager.Users.ToList()};

            return View(model);
        }
    }
}
