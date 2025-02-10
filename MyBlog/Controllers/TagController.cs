using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Entities;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece admin yetkisi olanlar erişebilir
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // 📌 **Tüm etiketleri listeleme**
        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }

        // 📌 **Yeni etiket ekleme - GET**
        public IActionResult Create()
        {
            return View();
        }

        // 📌 **Yeni etiket ekleme - POST**
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            var result = await _tagService.CreateTagAsync(tag);
            TempData["Message"] = result ? "Etiket başarıyla eklendi." : "Etiket eklenemedi.";
            return RedirectToAction("Index");
        }

        // 📌 **Etiket düzenleme - GET**
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null) return NotFound();

            return View(tag);
        }

        // 📌 **Etiket düzenleme - POST**
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            var result = await _tagService.UpdateTagAsync(tag);
            TempData["Message"] = result ? "Etiket başarıyla güncellendi." : "Etiket güncellenemedi.";
            return RedirectToAction("Index");
        }

        // 📌 **Etiket silme işlemi**
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);
            TempData["Message"] = result ? "Etiket başarıyla silindi." : "Etiket silinemedi.";
            return RedirectToAction("Index");
        }
    }
}
