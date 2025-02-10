using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Entities;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin yetkisi olanlar erişebilir
    public class BlogController : Controller
    {
        private readonly IPostService _postService;

        public BlogController(IPostService postService)
        {
            _postService = postService; // Bağımlılık enjeksiyonu
        }

        // Onay bekleyen blog yazılarını listeleme
        public async Task<IActionResult> BlogOnayListesi()
        {
            var posts = await _postService.GetPendingPostsAsync();
            return View("~/Views/Post/BlogOnayListesi.cshtml", posts); // Doğru görünüm yolu
        }

        // Tüm blog yazılarını listeleme
        public async Task<IActionResult> BlogListele()
        {
            var posts = await _postService.GetAllPostsAsync();
            return View("~/Views/Post/BlogListele.cshtml", posts); // Doğru görünüm yolu
        }

        [HttpPost]
        public async Task<IActionResult> Onayla(int id)
        {
            var result = await _postService.ApprovePostAsync(id);
            TempData["Message"] = result ? "Yazı onaylandı." : "Yazı onaylanamadı.";
            return RedirectToAction("BlogOnayListesi");
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var result = await _postService.DeletePostAsync(id);
            TempData["Message"] = result ? "Yazı silindi." : "Yazı silinemedi.";
            return RedirectToAction("BlogListele");
        }

        [HttpGet]
        public async Task<IActionResult> Guncelle(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(Post post)
        {
            if (!ModelState.IsValid) return View(post);

            var result = await _postService.UpdatePostAsync(post);
            TempData["Message"] = result ? "Yazı güncellendi." : "Yazı güncellenemedi.";
            return RedirectToAction("BlogListele");
        }

        [HttpPost]
        public async Task<IActionResult> Yayimla(int id)
        {
            var result = await _postService.PublishPostAsync(id);
            TempData["Message"] = result ? "Yazı yayınlandı." : "Yazı yayınlanamadı.";
            return RedirectToAction("BlogListele");
        }
    }
}
