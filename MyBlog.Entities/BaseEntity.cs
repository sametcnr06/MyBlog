using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities
{
    /// <summary>
    /// Tüm entity sınıfları için ortak özellikleri içerir.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; } // Veritabanındaki birincil anahtar.
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Kayıt oluşturulma tarihi.
        public DateTime? UpdatedDate { get; set; } // Kayıt güncellenme tarihi.
    }
}

