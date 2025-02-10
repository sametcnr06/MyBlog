using MyBlog.Business.Abstract;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Business.Concrete
{     
    /// Kategori yönetimi için servis sınıfı.     
    public class CategoryManager : ICategoryService
    {
        private readonly MyBlogContext _context;

        public CategoryManager(MyBlogContext context)
        {
            _context = context; // Dependency Injection ile bağlanıyor.
        }         
        /// Tüm kategorileri getirir.         
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }         
        /// Belirtilen ID'ye sahip kategoriyi getirir.         
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }         
        /// Yeni kategori ekler.         
        public async Task<bool> CreateCategoryAsync(Category category)
        {
            if (category == null)
                return false;

            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }         
        /// Kategori günceller.         
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            if (category == null)
                return false;

            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }         
        /// Belirtilen ID'ye sahip kategoriyi siler.        
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
