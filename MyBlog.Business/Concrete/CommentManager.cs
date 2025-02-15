using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly MyBlogContext _context;

        public CommentManager(MyBlogContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .Include(c => c.ApprovedByUser)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetUnapprovedCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .Where(c => !c.IsApproved)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        // 📌 Post ID'ye göre yorumları getir (Eksik olan metot eklendi)
        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Include(c => c.User) // Yorumu yazan kullanıcı
                .Where(c => c.PostId == postId && c.IsApproved) // Onaylanmış yorumları getiriyoruz
                .ToListAsync();
        }

        public async Task<bool> AddCommentAsync(Comment comment)
        {
            if (comment == null)
                return false;

            await _context.Comments.AddAsync(comment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ApproveCommentAsync(int commentId, string approvedByUserId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
                return false;

            comment.IsApproved = true;
            comment.ApprovedByUserId = approvedByUserId;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Comment>> GetCommentsByPostAuthorIdAsync(string authorId)
        {
            return await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .Where(c => c.Post.AuthorId == authorId)
                .ToListAsync();
        }
    }
}
