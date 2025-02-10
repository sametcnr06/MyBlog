using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyBlog.Entities.Identity;

namespace MyBlog.Entities
{
    /// <summary>
    /// Blog yazılarını temsil eder.
    /// </summary>
    public class Post : BaseEntity
    {
        [Required] // Bu alanın doldurulması zorunludur.
        [StringLength(100)] // Bu alanın maksimum uzunluğu 100 karakter ile sınırlandırılmıştır.
        public string Title { get; set; } = string.Empty; // Yazının başlığı.

        [Required] // Bu alanın doldurulması zorunludur.
        public string Content { get; set; } = string.Empty; // Yazının içeriği.

        public bool IsApproved { get; set; } // Yazının admin tarafından onaylanıp onaylanmadığını belirtir.

        public DateTime? PublishedDate { get; set; } // Yazının yayınlanma tarihi.

        public string AuthorId { get; set; } // Yazının yazarının ID'si. (string olarak güncellendi çünkü ApplicationUser'ın ID'si string)
        public ApplicationUser Author { get; set; } // Yazının yazarını temsil eder.

        public int CategoryId { get; set; } // Yazının kategorisinin ID'si.
        public Category Category { get; set; } // Yazının bağlı olduğu kategori.

        public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Yazıya yapılan tüm yorumlar.
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>(); // Yazıya bağlı tüm etiketler.
    }
}
