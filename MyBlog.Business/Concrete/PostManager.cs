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
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .Where(p => !p.IsApproved && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ApprovePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.IsDeleted) return false;

            post.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(int id, string writerId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.AuthorId != writerId) return false; // Yetkilendirme kontrolü

            post.IsDeleted = true;
            post.DeletedDate = DateTime.Now;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            if (post.IsDeleted) return false;

            _context.Posts.Update(post);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> PublishPostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.IsDeleted) return false;

            post.PublishedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Post>> GetPostsByAuthorAsync(string authorId, bool includeTags = false)
        {
            var query = _context.Posts.Where(p => p.AuthorId == authorId && !p.IsDeleted);
            if (includeTags)
            {
                query = query.Include(p => p.PostTags).ThenInclude(pt => pt.Tag);
            }
            return await query.ToListAsync();
        }

        public async Task<bool> AddPostAsync(Post post)
        {
            if (post == null) return false;

            await _context.Posts.AddAsync(post);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Post>> SearchPostsAsync(string authorName, string categoryName, string tagName)
        {
            var query = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .Where(p => p.IsApproved && !p.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(authorName))
            {
                query = query.Where(p => p.Author.UserName.Contains(authorName));
            }
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Category.CategoryName.Contains(categoryName));
            }
            if (!string.IsNullOrEmpty(tagName))
            {
                query = query.Where(p => p.PostTags.Any(pt => pt.Tag.Name.Contains(tagName)));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Post>> GetAllApprovedPostsAsync(bool includeTags = false)
        {
            IQueryable<Post> query = _context.Posts.Where(p => p.IsApproved && !p.IsDeleted);
            query = query.Include(p => p.Author)
                         .Include(p => p.Category);

            if (includeTags)
            {
                query = query.Include(p => p.PostTags).ThenInclude(pt => pt.Tag);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> RestorePostAsync(int id, string writerId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || !post.IsDeleted || post.AuthorId != writerId) return false; // Yetkilendirme kontrolü

            post.IsDeleted = false;
            post.DeletedDate = null;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Post>> GetArchivedPostsByAuthorAsync(string authorId)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .Where(p => p.AuthorId == authorId && p.IsDeleted)
                .ToListAsync();
        }

        // 📌 Yeni metod: Takip edilen yazarın yazılarını getir
        public async Task<List<Post>> GetPostsByAuthorIdAsync(string authorId, bool includeTags = false)
        {
            var query = _context.Posts.Where(p => p.AuthorId == authorId && !p.IsDeleted);

            if (includeTags)
            {
                query = query.Include(p => p.PostTags).ThenInclude(pt => pt.Tag);
            }

            return await query.ToListAsync();
        }
    }
}
