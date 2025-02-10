using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace MyBlog.Entities
{
    /// <summary>
    /// Blog yazılarının kategorilerini temsil eder.
    /// </summary>
    public class Category : BaseEntity
    {
        [Required] // Bu alanın doldurulması zorunludur.
        [StringLength(50)] // Bu alanın maksimum uzunluğu 50 karakter ile sınırlandırılmıştır.
        public string CategoryName { get; set; } = string.Empty; // Kategorinin adı.

        public string? Description { get; set; } // Kategorinin açıklaması (isteğe bağlı).
    }
}

