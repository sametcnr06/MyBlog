using MyBlog.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    // Etiket servis arayüzü. Tüm etiket işlemlerini içerir.
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync();   // Tüm etiketleri getir.
        Task<Tag> GetTagByIdAsync(int id);   // Belirtilen ID'ye göre etiket getir.
        Task<bool> CreateTagAsync(Tag tag);  // Yeni etiket ekle.
        Task<bool> UpdateTagAsync(Tag tag);  // Etiketi güncelle.
        Task<bool> DeleteTagAsync(int id);   // Etiketi sil.
    }
}
