using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Entities
{
    // Blog yazılarına ait etiketleri temsil eder.
    public class Tag : BaseEntity
    {
        // Bu alanın zorunlu olduğunu belirtir. Boş bırakılırsa validasyon hatası oluşur.
        [Required(ErrorMessage = "Etiket adı boş bırakılamaz.")]
        // Bu alanın maksimum uzunluğu 30 karakter ile sınırlandırılmıştır.
        [StringLength(30, ErrorMessage = "Etiket adı en fazla 30 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        // Etiketin açıklaması (isteğe bağlı).
        public string? Description { get; set; }

        // Etiketin bağlı olduğu yazılar (PostTag tablosu üzerinden many-to-many).
        public ICollection<PostTag> PostTags { get; set; }
            = new List<PostTag>();

    }
}
