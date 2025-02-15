using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Entities;
using MyBlog.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public HomeController(ILogger<HomeController> logger,
                              IPostService postService,
                              ICommentService commentService)
        {
            _logger = logger;
            _postService = postService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllApprovedPostsAsync(includeTags: true);

            if (posts == null)
            {
                posts = new List<Post>();
            }

            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }

        // 📌 Post detay sayfası (Artık yorumları getiriyor!)
        public async Task<IActionResult> PostDetail(int id)
        {
            if (id <= 0) return NotFound(); // 📌 Geçersiz ID için 404 döndür.

            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();

            // 📌 Yorumları yükle
            post.Comments = await _commentService.GetCommentsByPostIdAsync(id);

            return View(post);
        }
    }
}
