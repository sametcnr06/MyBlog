using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    // Hem Admin hem Editör yetkisi olanlar buradaki aksiyonları görebilsin
    [Authorize(Roles = "Admin,Editor")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Onay bekleyen yorumları listele
        public async Task<IActionResult> UnapprovedComments()
        {
            var comments = await _commentService.GetUnapprovedCommentsAsync();
            return View(comments); // Views/Comment/UnapprovedComments.cshtml
        }

        // Tüm yorumları listele (isteğe bağlı)
        public async Task<IActionResult> AllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return View(comments); // Views/Comment/AllComments.cshtml
        }

        // Yorum onaylama işlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Approve(int commentId)
        {
            // Şu an giriş yapan kullanıcının Id'si
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool result = await _commentService.ApproveCommentAsync(commentId, userId);
            if (result)
            {
                TempData["SuccessMessage"] = "Yorum başarıyla onaylandı.";
            }
            else
            {
                TempData["ErrorMessage"] = "Yorum onaylanırken bir hata oluştu.";
            }

            return RedirectToAction("UnapprovedComments");
        }

        // Yorum silme işlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Delete(int commentId)
        {
            bool result = await _commentService.DeleteCommentAsync(commentId);
            if (result)
            {
                TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Yorum silinirken bir hata oluştu.";
            }

            // Hangi sayfaya döneceğin sana bağlı (onay listesi veya tüm yorumlar?)
            return RedirectToAction("UnapprovedComments");
        }
    }
}
