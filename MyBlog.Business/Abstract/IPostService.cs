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
        Task<bool> DeletePostAsync(int id); // Yazıyı sil
        Task<bool> UpdatePostAsync(Post post); // Yazıyı güncelle
        Task<bool> PublishPostAsync(int id); // Yazıyı yayınla
    }
}
