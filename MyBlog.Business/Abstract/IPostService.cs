using MyBlog.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    public interface IPostService
    {
        Task<List<Post>> GetPendingPostsAsync(); // Onay bekleyen yazıları getir
        Task<List<Post>> GetAllPostsAsync(); // Tüm yazıları getir
        Task<Post> GetPostByIdAsync(int id); // Belirli bir yazıyı getir
        Task<bool> ApprovePostAsync(int id); // Yazıyı onayla
        Task<bool> DeletePostAsync(int id, string writerId); // Yazıyı sil (Soft Delete) - Sadece yazarına izin ver
        Task<bool> UpdatePostAsync(Post post); // Yazıyı güncelle
        Task<bool> PublishPostAsync(int id); // Yazıyı yayınla
        Task<List<Post>> GetPostsByAuthorAsync(string authorId, bool includeTags = false); // Yazarı ile yazıları getir
        Task<bool> AddPostAsync(Post post); // Yazı ekle
        Task<List<Post>> SearchPostsAsync(string authorName, string categoryName, string tagName); // Yazı ara
        Task<List<Post>> GetAllApprovedPostsAsync(bool includeTags = false); // Onaylanan yazıları getir
        Task<bool> RestorePostAsync(int id, string writerId); // Soft delete yapılmış yazıyı geri getir (Sadece yazarına izin ver)
        Task<List<Post>> GetArchivedPostsByAuthorAsync(string authorId); // Yumuşak silinmiş yazıları getir
        Task<List<Post>> GetPostsByAuthorIdAsync(string authorId, bool includeTags = false); // 📌 Belirtilen yazarın yazılarını getir (Eklendi)
    }
}
