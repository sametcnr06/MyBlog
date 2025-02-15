using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;   // IPostService, IFavoriteService, INotificationService
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize(Roles = "Subscriber")]
public class SubscriberController : Controller
{
    private readonly IPostService _postService;
    private readonly IFavoriteService _favoriteService;
    private readonly INotificationService _notificationService;
    private readonly MyBlogContext _context; // 📌 Context değişkeni eklendi

    // 📌 Constructor: Servisleri ve Context'i enjekte et
    public SubscriberController(
        IPostService postService,
        IFavoriteService favoriteService,
        INotificationService notificationService,
        MyBlogContext context) // 📌 Context parametre olarak alındı
    {
        _postService = postService;
        _favoriteService = favoriteService;
        _notificationService = notificationService;
        _context = context; // 📌 Context ataması yapıldı
    }

    // 📌 Tüm Yazılar + Arama
    // GET /Subscriber/Index
    public async Task<IActionResult> Index(string searchAuthor, string searchCategory, string searchTag)
    {
        var posts = await _postService.SearchPostsAsync(searchAuthor, searchCategory, searchTag);

        // 📌 Kullanıcının takip ettiği yazarları al
        var subscriberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var followedUsers = await _favoriteService.GetFollowedUsersAsync(subscriberId);

        // 📌 Takip edilen yazar ID'lerini ViewBag içine ekle
        ViewBag.FollowedAuthors = followedUsers.Select(u => u.Id).ToList();

        return View(posts);
    }

    // 📌 Yazar takip etme
    // POST /Subscriber/FollowAuthor
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FollowAuthor(string followedUserId)
    {
        var subscriberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(subscriberId) || string.IsNullOrEmpty(followedUserId))
        {
            TempData["ErrorMessage"] = "Takip işlemi yapılamadı (eksik veri).";
            return RedirectToAction("Index");
        }

        var result = await _favoriteService.FollowUserAsync(subscriberId, followedUserId);
        if (result)
        {
            // 📌 Bildirim gönder => "takip edildiniz"
            await _notificationService.SendNotificationAsync(
                followedUserId,
                "Bir kullanıcı sizi takip etti.",
                NotificationType.Follow
            );
            TempData["SuccessMessage"] = "Kullanıcı takip edildi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Takip işlemi gerçekleştirilemedi.";
        }

        return RedirectToAction("Index");
    }

    // 📌 Takip edilen yazarların listesi
    // GET /Subscriber/Follows
    [HttpGet]
    public async Task<IActionResult> Follows()
    {
        var subscriberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 📌 Takip edilen yazarları getir
        var followedUsers = await _favoriteService.GetFollowedUsersAsync(subscriberId);

        // 📌 Her yazarın yazılarını getir
        var followedAuthorsWithPosts = new List<object>();

        foreach (var user in followedUsers)
        {
            var posts = await _postService.GetPostsByAuthorAsync(user.Id, includeTags: true);
            followedAuthorsWithPosts.Add(new
            {
                Author = user,
                Posts = posts
            });
        }

        return View(followedAuthorsWithPosts);
    }

    // 📌 Takibi bırakma işlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnfollowAuthor(string authorId)
    {
        var subscriberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(subscriberId) || string.IsNullOrEmpty(authorId))
        {
            TempData["ErrorMessage"] = "Takibi bırakma işlemi başarısız.";
            return RedirectToAction("Follows");
        }

        var result = await _favoriteService.UnfollowUserAsync(subscriberId, authorId);
        if (result)
        {
            TempData["SuccessMessage"] = "Takip bırakıldı.";
        }
        else
        {
            TempData["ErrorMessage"] = "Takibi bırakma işlemi başarısız.";
        }

        return RedirectToAction("Follows");
    }

    // 📌 Belirli bir yazının detaylarını görüntüleme
    // GET /Subscriber/PostDetail/{id}
    [HttpGet]
    public async Task<IActionResult> PostDetail(int id)
    {
        if (id <= 0)
        {
            return NotFound(); // Geçersiz ID kontrolü
        }

        // 📌 Yazıyı veritabanından çek
        var post = await _postService.GetPostByIdAsync(id);

        if (post == null)
        {
            return NotFound(); // Yazı bulunamadı
        }

        // 📌 Kullanıcının yazarı takip edip etmediğini kontrol et
        var subscriberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isFollowing = await _favoriteService.IsFollowingAsync(subscriberId, post.AuthorId);

        // 📌 ViewBag içine ekleyelim (görünüme iletmek için)
        ViewBag.WriterId = subscriberId;
        ViewBag.IsFollowing = isFollowing;

        return View(post); // PostDetail.cshtml dosyasına yönlendir
    }
    // 📌 Yorum Ekleme İşlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int postId, string commentText)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Oturumdaki kullanıcı ID
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account"); // Giriş yapmayan kullanıcıyı yönlendir
        }

        if (postId <= 0 || string.IsNullOrEmpty(commentText))
        {
            TempData["ErrorMessage"] = "Yorum eklenemedi. Eksik veri!";
            return RedirectToAction("PostDetail", new { id = postId });
        }

        var comment = new Comment
        {
            PostId = postId,
            UserId = userId,
            Content = commentText,
            CreatedDate = DateTime.Now
        };

        // 📌 Veritabanına ekleme işlemi
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Yorum başarıyla eklendi!";
        return RedirectToAction("PostDetail", new { id = postId });
    }
}
