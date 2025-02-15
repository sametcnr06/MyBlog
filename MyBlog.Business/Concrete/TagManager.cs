using MyBlog.Business.Abstract;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Business.Concrete
{
    // Etiket yönetimi için servis sınıfı.
    public class TagManager : ITagService
    {
        private readonly MyBlogContext _context;

        public TagManager(MyBlogContext context)
        {
            _context = context; // Dependency Injection ile bağlanıyor.
        }
        // Tüm etiketleri getirir.
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }
        // Belirtilen ID'ye sahip etiketi getirir.
        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }
        // Yeni etiket ekler.
        public async Task<bool> CreateTagAsync(Tag tag)
        {
            if (tag == null)
                return false;

            _context.Tags.Add(tag);
                var result = await _context.SaveChangesAsync();
                return result > 0; // Başarılıysa true döner
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}"); // Hataları logla
                return false;
            }
        }
        // Etiketi günceller.
        public async Task<bool> UpdateTagAsync(Tag tag)
        {
            if (tag == null)
                return false;

            _context.Tags.Update(tag);
            return await _context.SaveChangesAsync() > 0;
        }
        // Belirtilen ID'ye sahip etiketi siler.
        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return false;

            _context.Tags.Remove(tag);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
