using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using WebApp.Common;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = model.Name,
                Email = model.Email
            }, model.Password);

            var adm = Constants.USER_ROLE_ADMIN;
            if(!await _roleManager.RoleExistsAsync(adm))
                await _roleManager.CreateAsync(new IdentityRole(adm));

            if (model.IsAdmin)
            {
                var user = await _userManager.FindByNameAsync(model.Name);
                await _userManager.AddToRoleAsync(user, adm);
            }

            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);

            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            var callbackUrl = Url.Action(nameof(HomeController.Index), "Home", values: null, protocol: Request.Scheme);

            return Redirect("~/Home/Index");

            return SignOut(
                new AuthenticationProperties {RedirectUri = callbackUrl},
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new {userId = user.Id, code},
                HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Reset Password",
                $"Link to reset your password: {callbackUrl}");

            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userid, string code)
        {
            var user = await _userManager.FindByIdAsync(userid);
            var model = new ResetPasswordViewModel {Code = code, Email = user.Email};

            return View(model);
        }

        [HttpGet]
        public IActionResult AzureAd()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Index), "Home");
            return Challenge(
                new AuthenticationProperties {RedirectUri = redirectUrl},
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            await _userManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);

            return Redirect("~/Home/Index");
        }

    }
}