using MyBlog.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    /// Etiket servis arayüzü. Tüm etiket işlemlerini içerir.
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync(); // Tüm etiketleri getirir.
        Task<Tag> GetTagByIdAsync(int id); // Belirtilen ID'ye göre etiket getirir.
        Task<bool> CreateTagAsync(Tag tag); // Yeni etiket ekler.
        Task<bool> UpdateTagAsync(Tag tag); // Etiketi günceller.
        Task<bool> DeleteTagAsync(int id); // Etiketi siler.
    }
}
