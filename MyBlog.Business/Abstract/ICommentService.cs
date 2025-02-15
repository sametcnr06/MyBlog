using MyBlog.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    public interface ICommentService
    {
        // Tüm yorumları getir
        Task<List<Comment>> GetAllCommentsAsync();

        // Onay bekleyen (IsApproved = false) yorumları getir
        Task<List<Comment>> GetUnapprovedCommentsAsync();

        // Belirli bir yorum ID'sine sahip yorumu getir
        Task<Comment> GetCommentByIdAsync(int commentId);

        // 📌 Belirli bir Post ID'ye ait yorumları getir (Eksik olan metod eklendi)
        Task<List<Comment>> GetCommentsByPostIdAsync(int postId);

        // Yeni bir yorum ekle
        Task<bool> AddCommentAsync(Comment comment);

        // Yorum onaylama işlemi
        Task<bool> ApproveCommentAsync(int commentId, string approvedByUserId);

        // Yorum silme işlemi
        Task<bool> DeleteCommentAsync(int commentId);

        // 📌 Yazarın yazılarına gelen yorumları getir
        Task<List<Comment>> GetCommentsByPostAuthorIdAsync(string authorId);
    }
}
