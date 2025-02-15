using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Entities;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Authorize(Roles = "Admin,Editor")] // Sadece Admin ve Editor yetkisi olanlar erişebilir.
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Tüm kategorileri listeleme
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // Yeni kategori ekleme (GET - Form Göster)
        public IActionResult Create()
        {
            return View();
        }

        // Yeni kategori ekleme (POST - Verileri Kaydet)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryService.CreateCategoryAsync(category);
            TempData["SuccessMessage"] = "Kategori başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        // Kategori düzenleme (GET - Form Göster)
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Kategori güncelleme (POST - Verileri Güncelle)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryService.UpdateCategoryAsync(category);
            TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // Kategori silme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            TempData["Message"] = result ? "Kategori başarıyla silindi." : "Kategori silinemedi.";
            return RedirectToAction("Index");
        }
    }
}
