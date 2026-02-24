using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WardDapperMVC.Repository;
using WardDapperMVC.Models.Domain;
using Microsoft.Extensions.Logging;

namespace WardManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepo _loginRepo;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginRepo loginRepo, ILogger<LoginController> logger)
        {
            _loginRepo = loginRepo;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Your user login logic
                var user = await _loginRepo.Login(model.Email, model.Password);
                var userRole = await _loginRepo.GetUserRoleByEmailAsync(model.Email);
                var userName = await _loginRepo.GetUserNameByEmailAsync(model.Email);

                if (user != null)
                {
                    var claims = new[]
                    {
                         new Claim(ClaimTypes.Email, model.Email),
                         new Claim(ClaimTypes.Name, userName),
                         new Claim(ClaimTypes.Role, userRole)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false, // This makes it a session cookie
                        ExpiresUtc = null // No expiration; it defaults to session
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                    // Redirect based on user role
                    return userRole == "NursingSister"
                    ? RedirectToAction("Home", nameof(Nurse))
                    : RedirectToAction("Home", userRole);
                }

                ModelState.AddModelError(string.Empty, "Email or password is invalid.");
            }

            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ValidationPage()
        {
            return View();
        }

        public IActionResult NewPassword()
        {
            return View();
        }
    }
}



