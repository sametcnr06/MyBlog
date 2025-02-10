using MyBlog.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{    
    // Kategori servis arayüzü. Tüm kategori işlemlerini içerir.    
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync(); // Tüm kategorileri getirir.
        Task<Category> GetCategoryByIdAsync(int id); // Belirtilen ID'ye göre kategori getirir.
        Task<bool> CreateCategoryAsync(Category category); // Yeni kategori ekler.
        Task<bool> UpdateCategoryAsync(Category category); // Kategori günceller.
        Task<bool> DeleteCategoryAsync(int id); // Kategori siler.
    }
}
