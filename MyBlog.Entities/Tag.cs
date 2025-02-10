using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Entities
{
    /// <summary>
    /// Blog yazılarına ait etiketleri temsil eder.
    /// </summary>
    public class Tag : BaseEntity
    {
        [Required] // Bu alanın zorunlu olduğunu belirtir. Boş bırakılırsa validasyon hatası oluşur.
        [StringLength(30)] // Bu alanın maksimum uzunluğu 30 karakter ile sınırlandırılmıştır.
        public string Name { get; set; } = string.Empty; // Etiketin adı.

        public string? Description { get; set; } // Etiketin açıklaması (isteğe bağlı).

        public ICollection<PostTag> PostTags { get; set; } // Etiketin bağlı olduğu yazılar.
    }
}

