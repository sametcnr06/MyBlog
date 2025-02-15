using MyBlog.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Entities
{
    
    // Blog yazılarına yapılan yorumları temsil eder.
    
    public class Comment : BaseEntity
    {
        [Required] // Bu alanın doldurulması zorunludur.
        public string Content { get; set; } = string.Empty; // Yorumun içeriği.

        public bool IsApproved { get; set; } = false; // Yorumun onaylanıp onaylanmadığını belirtir.

        public int PostId { get; set; } // Yorumun ait olduğu yazının ID'si.
        public Post Post { get; set; } // Yorumun ait olduğu yazı.

        public string UserId { get; set; } // Yorumu yazan kullanıcının ID'si.
        public ApplicationUser User { get; set; } // Yorumu yazan kullanıcı.

        public string? ApprovedByUserId { get; set; } // Yorumu onaylayan adminin ID'si (isteğe bağlı, string olarak güncellendi).
        public ApplicationUser? ApprovedByUser { get; set; } // Yorumu onaylayan admin (isteğe bağlı).
    }
}
