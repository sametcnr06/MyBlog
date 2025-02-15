using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Models.ViewModels;

namespace MyBlog.Controllers
{
    // Yalnızca Admin yetkisi olan kullanıcıların erişebileceği bir controller
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        // Dependency Injection ile IRoleService enjekte ediliyor
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Tüm rolleri listeleme
        public IActionResult Index()
        {
            var roles = _roleService.GetList();
            return View(roles);

            //var roles = await _roleService.GetAllRolesAsync();

            //var roleViewModels = roles.Select(role => new RoleViewModel
            //{
            //    Name = role.Name,
            //    UserCount = role.Users?.Count ?? 0 // Kullanıcı sayısını alırken null kontrolü
            //}).ToList();

            //return View(roleViewModels);
        }

        //var patientList = _patientDao.GetByMotherChildList().GroupBy(x=>x.CreatedDate).
    //    Select(group => new PatientStatisticDto
    //            {
    //                Date = group.Key, //Tarihe göre gruplama
    //                MotherCount = group.Count(x => x.MotherId == null), // Anne sayısı
    //                ChildCount = group.Count(x => x.MotherId != null), // Bebek sayısı
    //                TotalCount = group.Count() // Anne-bebek toplam sayı
    //})
    //            .OrderBy(x => x.Date) // Tarih sıralaması
    //            .ToList();

    [HttpGet]
        public IActionResult YeniRolEkle()
        {
            return View();
        }

        // Yeni rol oluşturma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YeniRolEkle(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Rol adı boş olamaz.");
                return View();
            }

            var result = await _roleService.CreateRoleAsync(roleName);
            TempData["Message"] = result ? "Rol başarıyla oluşturuldu." : "Rol oluşturulamadı.";

            //var userRole = _roleService.CreateRoleUserAsync(roleName);


            return RedirectToAction("Index");
        }

        // Rol silme işlemi
        public async Task<IActionResult> Delete(string roleName)
        {
            // Verilen rolü sil ve sonucu kontrol et
            var result = await _roleService.DeleteRoleAsync(roleName);
            TempData["Message"] = result ? "Rol başarıyla silindi." : "Rol silinemedi.";

            return RedirectToAction("Index");
        }

        // Kullanıcıları belirli bir role göre filtreleme
        [HttpGet]
        public async Task<IActionResult> Filtrele(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                TempData["Message"] = "Lütfen bir rol seçin.";
                return RedirectToAction("Index");
            }

            // Role adına göre kullanıcıları getir
            var usersInRole = await _roleService.GetUsersByRoleAsync(roleName);

            // Kullanıcıları ViewModel'e dönüştür
            var userViewModels = usersInRole.Select(user => new UserViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            }).ToList();

            // Role ve kullanıcıları View'a gönder
            ViewBag.SelectedRole = roleName;
            return View("FiltreliKullanicilar", userViewModels);
        }


        public IActionResult GetRoleListCount()
        {
            var roles =  _roleService.GetList();
            return View(roles);
        }

    }
}
