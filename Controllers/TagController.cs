using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Entities;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    // Etiket (Tag) yönetimi için Controller. Sadece Admin ve Editor rolüne açıktır.
    [Authorize(Roles = "Admin,Editor")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // Tüm etiketleri listeleyen action. GET /Tag/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }

        // Yeni etiket ekleme formunu gösterir. GET /Tag/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Yeni etiket ekleme işlemi (POST). /Tag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            var result = await _tagService.CreateTagAsync(tag);
            if (result)
            {
                TempData["SuccessMessage"] = "Etiket başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Etiket eklenirken bir hata oluştu.";
            return View(tag);
        }

        // Etiket düzenleme formu (GET). /Tag/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // Etiket düzenleme işlemi (POST). /Tag/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            var result = await _tagService.UpdateTagAsync(tag);
            if (result)
            {
                TempData["SuccessMessage"] = "Etiket başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Etiket güncellenirken bir hata oluştu.";
            return View(tag);
        }

        // Etiket silme işlemi (POST). /Tag/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Etiket başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Etiket silinirken bir hata oluştu.";
            }

            return RedirectToAction("Index");
        }
    }
}
