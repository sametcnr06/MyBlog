using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entities.Identity;
using System.IO;

namespace MyBlog.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager; // Dependency injection
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string firstName, string lastName, string email, string username, string password, IFormFile photoFile)
        {
            if (!ModelState.IsValid || photoFile == null || photoFile.Length == 0)
            {
                ModelState.AddModelError("", "Tüm alanları doldurmanız ve bir fotoğraf seçmeniz gerekiyor.");
                return View();
            }

            // Fotoğrafın kaydedilmesi
            string fileName = Guid.NewGuid() + Path.GetExtension(photoFile.FileName);
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photoFile.CopyToAsync(stream);
            }

            // Kullanıcı oluşturma
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PhotoPath = $"/uploads/{fileName}", // Fotoğraf yolu
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Kullanıcıya varsayılan olarak "Subscriber" rolünü ata
                await _userManager.AddToRoleAsync(user, "Subscriber");
                return RedirectToAction("Index", "Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }
    }
}
