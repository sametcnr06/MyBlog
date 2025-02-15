using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Concrete
{
    // Etiket yönetimi için servis sınıfı.
    public class TagManager : ITagService
    {
        private readonly MyBlogContext _context;

        public TagManager(MyBlogContext context)
        {
            _context = context;
        }

        // Tüm etiketleri getir.
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        // Belirtilen ID'ye göre etiket getir.
        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        // Yeni etiket ekler.
        public async Task<bool> CreateTagAsync(Tag tag)
        {
            try
            {
                if (tag == null)
                    return false;

                await _context.Tags.AddAsync(tag);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Hataları logla veya yakala
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }

        // Etiketi günceller.
        public async Task<bool> UpdateTagAsync(Tag tag)
        {
            if (tag == null)
                return false;

            _context.Tags.Update(tag);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // Etiketi siler.
        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return false;

            _context.Tags.Remove(tag);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
