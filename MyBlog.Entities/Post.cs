using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyBlog.Entities.Identity;

namespace MyBlog.Entities
{
    // Blog yazılarını temsil eder.
    public class Post : BaseEntity
    {
        [Required] // Bu alanın doldurulması zorunludur.
        [StringLength(100)] // Bu alanın maksimum uzunluğu 100 karakter ile sınırlandırılmıştır.
        public string Title { get; set; } = string.Empty; // Yazının başlığı.

        [Required] // Bu alanın doldurulması zorunludur.
        public string Content { get; set; } = string.Empty; // Yazının içeriği.

        public bool IsApproved { get; set; } // Yazının admin tarafından onaylanıp onaylanmadığını belirtir.

        public DateTime? PublishedDate { get; set; } // Yazının yayınlanma tarihi.
        public string? ImageUrl { get; set; } // Yeni eklenen alan

        public string? AuthorId { get; set; } // Yazının yazarının ID'si. (string olarak güncellendi çünkü ApplicationUser'ın ID'si string)
        public ApplicationUser? Author { get; set; } // Yazının yazarını temsil eder.

        public int CategoryId { get; set; } // Yazının kategorisinin ID'si.
        public Category? Category { get; set; } // Yazının bağlı olduğu kategori.

        public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Yazıya yapılan tüm yorumlar.
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>(); // Yazıya bağlı tüm etiketler.

        public bool IsDeleted { get; set; } // Soft delete alanı

        public DateTime? DeletedDate { get; set; } // Soft delete işlemi için silinme tarihi
    }
}
