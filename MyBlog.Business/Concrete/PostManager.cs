using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Business.Concrete
{
    public class PostManager : IPostService
    {
        private readonly MyBlogContext _context;

        public PostManager(MyBlogContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPendingPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Author) // Yazarı dahil et
                .Include(p => p.Category) // Kategoriyi dahil et
                .Where(p => !p.IsApproved) // Onaylanmamış yazıları al
                .ToListAsync();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ApprovePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return false;

            post.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PublishPostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return false;

            post.PublishedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
