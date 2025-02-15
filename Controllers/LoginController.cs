using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entities.Identity;

namespace MyBlog.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string username, string password, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    // Kullanıcının rolünü al
                    var roles = await _userManager.GetRolesAsync(user);

                    // Role göre yönlendirme
                    if (roles.Contains("Admin"))
                        return RedirectToAction("Index", "Admin");
                    if (roles.Contains("Editor"))
                        return RedirectToAction("Index", "Editor");
                    if (roles.Contains("Writer"))
                        return RedirectToAction("Index", "Writer");
                    if (roles.Contains("Subscriber"))
                        return RedirectToAction("Index", "Subscriber");
                }

                return RedirectToAction("Index", "Home"); // Default yönlendirme
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz giriş denemesi.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
