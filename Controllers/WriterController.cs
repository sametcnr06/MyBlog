using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Abstract;
using MyBlog.Entities;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

[Authorize(Roles = "Writer")] // Yalnızca "Writer" rolündeki kullanıcılar erişebilir.
public class WriterController : Controller
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    private readonly IFavoriteService _favoriteService; // 📌 Takip etme servisi eklendi.
    private readonly INotificationService _notificationService; // 📌 Bildirim servisi eklendi

    public WriterController(IPostService postService,
                           ICommentService commentService,
                           ICategoryService categoryService,
                           ITagService tagService,
                           IFavoriteService favoriteService,
                           INotificationService notificationService) // 📌 Dependency Injection eklendi
    {
        _postService = postService;
        _commentService = commentService;
        _categoryService = categoryService;
        _tagService = tagService;
        _favoriteService = favoriteService;
        _notificationService = notificationService;
    }

    // **📌 Writer Anasayfası (Onaylanan yazıları göster)**
    [HttpGet]
    public async Task<IActionResult> Index(string searchAuthor, string searchCategory, string searchTag)
    {
        var posts = await _postService.SearchPostsAsync(searchAuthor, searchCategory, searchTag);

        // 📌 Kullanıcının takip ettiği yazarları al
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var followedUsers = await _favoriteService.GetFollowedUsersAsync(writerId);

        // 📌 Takip edilen yazar ID'lerini ViewBag içine ekle
        ViewBag.FollowedAuthors = followedUsers.Select(u => u.Id).ToList();
        ViewBag.WriterId = writerId; // 📌 Kullanıcı ID'sini View'a gönder

        return View(posts);
    }
    // **📌 Writer'ın kendi yazılarını listeleme**
    public async Task<IActionResult> MyPosts()
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini al
        var myPosts = await _postService.GetPostsByAuthorAsync(writerId);
        return View(myPosts);
    }

    // **📌 Yeni Yazı Ekleme Sayfası**
    [HttpGet]
    public async Task<IActionResult> CreatePost()
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Tags = await _tagService.GetAllTagsAsync();
        return View();
    }

    // **📌 Yeni Yazı Kaydetme İşlemi**
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePost(Post post, IFormFile ImageFile, List<int>? selectedTagIds = null)
    {
        post.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Yazarın ID'sini ata
        if (post.CategoryId == 0)
        {
            ModelState.AddModelError("CategoryId", "Kategori seçmek zorunludur!");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Tags = await _tagService.GetAllTagsAsync();
            return View(post);
        }

        // **Görsel yükleme işlemi**
        if (ImageFile != null)
        {
            var filePath = Path.Combine("wwwroot/uploads", ImageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }
            post.ImageUrl = "/uploads/" + ImageFile.FileName;
        }

        post.IsApproved = false; // Yeni yazılar onay bekleyecek
        if (selectedTagIds != null)
        {
            post.PostTags = selectedTagIds.Select(tagId => new PostTag { TagId = tagId }).ToList();
        }

        var success = await _postService.AddPostAsync(post);
        TempData["SuccessMessage"] = success ? "Yeni yazı oluşturuldu. Onay bekliyor." : "Yazı oluşturulurken hata oluştu.";
        return success ? RedirectToAction("MyPosts") : View(post);
    }

    // **📌 Yazıyı Güncelleme Sayfası**
    [HttpGet]
    public async Task<IActionResult> EditPost(int id)
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = await _postService.GetPostByIdAsync(id);

        if (post == null || post.AuthorId != writerId || post.IsDeleted)
            return Unauthorized();

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Tags = await _tagService.GetAllTagsAsync();
        return View(post);
    }

    // **📌 Yazıyı Güncelleme İşlemi**
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(Post post, IFormFile ImageFile, List<int>? selectedTagIds = null)
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var dbPost = await _postService.GetPostByIdAsync(post.Id);

        if (dbPost == null || dbPost.AuthorId != writerId || dbPost.IsDeleted)
            return Unauthorized();

        dbPost.Title = post.Title;
        dbPost.Content = post.Content;
        dbPost.CategoryId = post.CategoryId;
        dbPost.UpdatedDate = DateTime.Now;

        // **Görsel güncelleme işlemi**
        if (ImageFile != null)
        {
            var filePath = Path.Combine("wwwroot/uploads", ImageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }
            dbPost.ImageUrl = "/uploads/" + ImageFile.FileName;
        }

        dbPost.PostTags.Clear();
        if (selectedTagIds != null)
        {
            dbPost.PostTags = selectedTagIds.Select(tagId => new PostTag { PostId = dbPost.Id, TagId = tagId }).ToList();
        }

        var success = await _postService.UpdatePostAsync(dbPost);
        TempData["SuccessMessage"] = success ? "Yazı güncellendi (Onay bekliyor)." : "Yazı güncellenirken hata oluştu.";
        return RedirectToAction("MyPosts");
    }

    // **📌 Yazıyı Arşivleme (Soft Delete)**
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await _postService.DeletePostAsync(postId, writerId);
        TempData["SuccessMessage"] = success ? "Yazı başarıyla arşivlendi." : "Yazı arşivleme işlemi sırasında bir hata oluştu.";
        return RedirectToAction("MyPosts");
    }

    // **📌 Arşivlenmiş Yazıları Listeleme**
    [HttpGet]
    public async Task<IActionResult> ArchivedPosts()
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var archivedPosts = await _postService.GetArchivedPostsByAuthorAsync(writerId);
        return View(archivedPosts);
    }

    // **📌 Arşivlenmiş Yazıyı Geri Getirme**
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestorePost(int postId)
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await _postService.RestorePostAsync(postId, writerId);

        if (!success)
        {
            TempData["ErrorMessage"] = "Bu yazıyı geri getirme yetkiniz yok veya zaten aktif.";
            return RedirectToAction("ArchivedPosts");
        }

        TempData["SuccessMessage"] = "Yazı başarıyla geri getirildi.";
        return RedirectToAction("ArchivedPosts");
    }

    // **📌 Yazarın yazılarına gelen yorumları listeleme**
    public async Task<IActionResult> CommentsOnMyPosts()
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var comments = await _commentService.GetCommentsByPostAuthorIdAsync(writerId);
        return View(comments);
    }
    // 📌 Yazı Detay Sayfası (Takip Etme & Yorum Ekleme + Yorumları Getirme)
    [HttpGet]
    public async Task<IActionResult> PostDetail(int id)
    {
        // 📌 Geçersiz bir ID gelirse 404 hatası döndür
        if (id <= 0) return NotFound();

        // 📌 Post'u ID'ye göre getir
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null) return NotFound();

        // 📌 Yorumları veritabanından çek ve `post.Comments` içine ata
        post.Comments = await _commentService.GetCommentsByPostIdAsync(id);

        // 📌 Kullanıcının oturumdaki ID'sini al
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 📌 Kullanıcının bu yazının yazarını takip edip etmediğini kontrol et
        bool isFollowing = (await _favoriteService.GetFollowedUsersAsync(writerId))
                            .Any(u => u.Id == post.AuthorId);

        // 📌 ViewBag ile takip bilgilerini View'e gönderiyoruz
        ViewBag.IsFollowing = isFollowing;
        ViewBag.WriterId = writerId;

        // 📌 View'a Post nesnesini gönder
        return View(post);
    }


    // 📌 Kullanıcıyı Takip Et (Hatalar düzeltildi)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FollowUser(string followedUserId)
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (writerId == followedUserId) return BadRequest("Kendinizi takip edemezsiniz!");

        var result = await _favoriteService.FollowUserAsync(writerId, followedUserId);
        if (result)
        {
            // 📌 Takip edilen kullanıcıya bildirim gönder
            await _notificationService.SendNotificationAsync(
                followedUserId,
                "Bir kullanıcı sizi takip etti!",
                NotificationType.Follow
            );

            TempData["SuccessMessage"] = "Kullanıcı başarıyla takip edildi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Takip işlemi başarısız oldu.";
        }

        return Redirect(Request.Headers["Referer"].ToString()); // 📌 Kullanıcı hangi sayfadaysa oraya yönlendir
    }

    // 📌 Takibi Bırak (Hatalar düzeltildi)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnfollowUser(string followedUserId)
    {
        var writerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool result = await _favoriteService.UnfollowUserAsync(writerId, followedUserId);

        if (result)
        {
            TempData["SuccessMessage"] = "Takipten çıkıldı.";
        }
        else
        {
            TempData["ErrorMessage"] = "Takipten çıkma işlemi başarısız oldu.";
        }

        return Redirect(Request.Headers["Referer"].ToString()); // 📌 Kullanıcı hangi sayfadaysa oraya yönlendir
    }

    // 📌 Yorum Ekleme İşlemi (Hatalar düzeltildi)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int postId, string commentText)
    {
        if (string.IsNullOrWhiteSpace(commentText))
        {
            TempData["ErrorMessage"] = "Yorum boş olamaz.";
            return RedirectToAction("PostDetail", new { id = postId });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var comment = new Comment
        {
            PostId = postId,
            Content = commentText,
            UserId = userId,
            CreatedDate = DateTime.Now,
            IsApproved = false
        };

        var success = await _commentService.AddCommentAsync(comment);
        TempData["SuccessMessage"] = success ? "Yorum başarıyla eklendi, onay bekliyor." : "Yorum eklenemedi.";
        return RedirectToAction("PostDetail", new { id = postId });
    }
}

