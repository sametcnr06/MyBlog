using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.Identity;
using MyBlog.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Route("Admin/[action]")]
    [Authorize(Roles = "Admin")] // Yalnızca "Admin" rolüne sahip kullanıcılar bu controller'a erişebilir.
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; // Kullanıcı yönetimi için UserManager
        private readonly RoleManager<ApplicationRole> _roleManager; // Rol yönetimi için RoleManager

        // Constructor: Dependency Injection ile UserManager ve RoleManager hizmetlerini alıyoruz.
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 🏠 **Admin Paneli Ana Sayfası**
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard"; // Sayfa başlığını belirle
            return View(); // Admin paneline yönlendir
        }

        // 📌 **Tüm Kullanıcıları Listeleme**
        public async Task<IActionResult> KullaniciListesi()
        {
            var users = await _userManager.Users.ToListAsync(); // Tüm kullanıcıları getir
            var userList = new List<KullaniciViewModel>(); // Kullanıcı listesi için ViewModel oluştur

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Kullanıcının rollerini getir
                userList.Add(new KullaniciViewModel
                {
                    Id = user.Id, // Kullanıcı ID'si
                    FirstName = user.FirstName, // Kullanıcı adı
                    LastName = user.LastName, // Kullanıcı soyadı
                    Email = user.Email, // Kullanıcı e-postası
                    CreatedDate = user.CreatedDate, // Kullanıcının oluşturulma tarihi
                    UpdatedDate = user.UpdatedDate, // Kullanıcının son güncellenme tarihi
                    DeletedDate = user.DeletedDate, // Kullanıcının silinme tarihi (soft delete için)
                    IsDeleted = user.IsDeleted, // Kullanıcı silinmiş mi?
                    Roles = roles.ToList() // Kullanıcıya atanmış roller
                });
            }

            return View(userList); // Kullanıcı listesini View'e gönder
        }

        // 🆕 **Yeni Kullanıcı Ekleme Sayfasını Aç**
        public async Task<IActionResult> KullaniciEkle()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(); // Kullanılabilir rolleri getir
            return View(); // Kullanıcı ekleme sayfasını döndür
        }

        // 🆕 **Yeni Kullanıcı Ekleme İşlemi**
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması için
        public async Task<IActionResult> KullaniciEkle(KullaniciEkleViewModel model)
        {
            if (!ModelState.IsValid) // Model doğrulama başarısızsa
            {
                ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(); // Roller tekrar yükleniyor
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email, // Kullanıcı adı olarak e-posta atanıyor
                Email = model.Email, // Kullanıcının e-posta adresi
                FirstName = model.FirstName, // Kullanıcının adı
                LastName = model.LastName, // Kullanıcının soyadı
                CreatedDate = DateTime.Now, // Kullanıcının oluşturulma tarihi atanıyor
                IsActive = true // Varsayılan olarak kullanıcı aktif
            };

            var result = await _userManager.CreateAsync(user, model.Password); // Kullanıcı oluşturuluyor

            if (result.Succeeded) // Kullanıcı başarıyla oluşturulduysa
            {
                if (model.Roles != null && model.Roles.Any()) // Kullanıcıya roller atanacaksa
                {
                    await _userManager.AddToRolesAsync(user, model.Roles); // Kullanıcıya roller atanıyor
                }

                TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi."; // Başarı mesajı
                return RedirectToAction(nameof(KullaniciListesi)); // Kullanıcı listesine yönlendir
            }

            foreach (var error in result.Errors) // Hata varsa hata mesajlarını göster
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync(); // Roller tekrar yükleniyor
            return View(model);
        }

        // 📌 **Kullanıcı Düzenleme Sayfası**
        public async Task<IActionResult> Duzenle(string id)
        {
            var user = await _userManager.FindByIdAsync(id); // Kullanıcıyı bul
            if (user == null) // Kullanıcı bulunamazsa 404 döndür
            {
                return NotFound();
            }

            // Kullanıcı bilgilerini ViewModel'e map et
            var model = new KullaniciDuzenleViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive,
                ProfilePhotoPath = user.PhotoPath // Mevcut profil fotoğrafı yolu
            };

            return View(model); // Modeli View'e gönder
        }


        // 📌 **Kullanıcı Güncelleme İşlemi**
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması için
        public async Task<IActionResult> Duzenle(KullaniciDuzenleViewModel model)
        {
            if (!ModelState.IsValid) // Model doğrulama başarısızsa
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id); // Kullanıcıyı bul
            if (user == null)
            {
                return NotFound();
            }

            // Kullanıcı bilgilerini güncelle
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.IsActive = model.IsActive;
            user.UpdatedDate = DateTime.Now; // Güncelleme tarihi kaydediliyor

            // **Profil Fotoğrafı Güncelleme**
            if (model.ProfilePhoto != null)
            {
                var filePath = Path.Combine("wwwroot/uploads", model.ProfilePhoto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePhoto.CopyToAsync(stream);
                }
                user.PhotoPath = "/uploads/" + model.ProfilePhoto.FileName; // Fotoğraf yolu kaydediliyor
            }

            // **Şifre Değiştirme İşlemi**
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Şifreler uyuşmuyor!");
                    return View(model);
                }

                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(user, passwordResetToken, model.NewPassword);

                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var error in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction(nameof(KullaniciListesi)); // Kullanıcı listesine yönlendir
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }


        // 📌 **Rol Atama İşlemi**
        [HttpGet]
        public async Task<IActionResult> RolAtama(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new RolAtamaViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName, // Kullanıcının adı
                LastName = user.LastName,   // Kullanıcının soyadı
                AssignedRoles = userRoles.ToList(),
                AvailableRoles = roles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RolAtama(RolAtamaViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("KullaniciListesi");
            }

            var userRoles = await _userManager.GetRolesAsync(user); // Mevcut roller
            var selectedRoles = model.AssignedRoles ?? new List<string>();

            // Önce eski roller kaldırılır
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Yeni roller atanır
            await _userManager.AddToRolesAsync(user, selectedRoles);

            TempData["SuccessMessage"] = "Kullanıcının rolleri başarıyla güncellendi.";
            return RedirectToAction("KullaniciListesi");
        }
        // 📌 **Kullanıcı Detay Sayfası**
        public async Task<IActionResult> Detay(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.UserRoles) // Kullanıcının rollerini getir
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user); // Kullanıcının rollerini getir

            var model = new KullaniciDetayViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate,
                DeletedDate = user.DeletedDate,
                IsDeleted = user.IsDeleted,
                ProfilePhotoPath = user.PhotoPath ?? "/img/default-user.png", // Profil resmi yoksa varsayılan resmi kullan
                Roles = roles.ToList()
            };

            return View(model);
        }

        // Kullanıcıyı Soft Delete yaparak silme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması
        public async Task<IActionResult> Sil(string id)
        {
            var user = await _userManager.FindByIdAsync(id); // Kullanıcıyı bul
            if (user == null) // Kullanıcı yoksa 404 döndür
            {
                return NotFound();
            }

            // Kullanıcıyı soft delete olarak işaretle
            user.IsDeleted = true;
            user.DeletedDate = DateTime.Now; // Silinme tarihini kaydet

            var result = await _userManager.UpdateAsync(user); // Güncelleme işlemini yap

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla arşivlendi."; // Başarı mesajı
                return RedirectToAction(nameof(KullaniciListesi)); // Kullanıcı listesine yönlendirme
            }

            // Hata durumunda mesaj göster
            TempData["ErrorMessage"] = "Kullanıcı arşivleme işlemi sırasında bir hata oluştu.";
            return RedirectToAction(nameof(KullaniciListesi));
        }

        // Arşivlenmiş (silinmiş) kullanıcıları listeleme
        public async Task<IActionResult> ArsivlenmisKullanicilar()
        {
            // IsDeleted alanı true olan kullanıcıları getir
            var users = await _userManager.Users
                .Where(u => u.IsDeleted)
                .ToListAsync();

            var userList = new List<KullaniciViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Kullanıcı rollerini getir
                userList.Add(new KullaniciViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedDate = user.CreatedDate,
                    DeletedDate = user.DeletedDate,
                    Roles = roles.ToList(),
                    IsDeleted = user.IsDeleted
                });
            }

            return View(userList); // Arşivlenmiş kullanıcıları View'e gönder
        }

        // Arşivlenmiş kullanıcıyı geri alma (restore) işlemi
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF koruması
        public async Task<IActionResult> GeriAl(string id)
        {
            var user = await _userManager.FindByIdAsync(id); // Kullanıcıyı bul
            if (user == null) // Kullanıcı yoksa 404 döndür
            {
                return NotFound();
            }

            // Kullanıcının silinme durumunu geri al
            user.IsDeleted = false;
            user.DeletedDate = null; // Silinme tarihini sıfırla

            var result = await _userManager.UpdateAsync(user); // Güncelleme işlemini yap

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla geri alındı."; // Başarı mesajı
                return RedirectToAction(nameof(ArsivlenmisKullanicilar)); // Arşivlenmiş kullanıcılar listesine yönlendirme
            }

            // Hata durumunda mesaj göster
            TempData["ErrorMessage"] = "Kullanıcı geri alma işlemi sırasında bir hata oluştu.";
            return RedirectToAction(nameof(ArsivlenmisKullanicilar));
        }

        [HttpGet]
        public async Task<IActionResult> RolAtamaPartial(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new RolAtamaViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                AssignedRoles = userRoles.ToList(),
                AvailableRoles = roles
            };

            return PartialView("_RolAtamaModal", model); // PartialView döndür
        }

        // Profil Görüntüleme
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new AdminProfileViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoPath = user.PhotoPath,
                Role = "Admin" // Rol sabit
            };

            return View(model);
        }

        // Profil Güncelleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(AdminProfileViewModel model, IFormFile ProfilePhoto)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Profile");
            }

            // Eğer yeni bir fotoğraf seçilmişse
            if (ProfilePhoto != null && ProfilePhoto.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{ProfilePhoto.FileName}";
                var filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePhoto.CopyToAsync(stream);
                }

                // Eski fotoğrafı silme (isteğe bağlı)
                if (!string.IsNullOrEmpty(user.PhotoPath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                user.PhotoPath = $"/uploads/{uniqueFileName}";
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Profil başarıyla güncellendi.";
            }
            else
            {
                TempData["Error"] = "Profil güncellenirken bir hata oluştu.";
            }

            return RedirectToAction("Profile");
        }

    }
}
