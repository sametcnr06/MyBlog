using System;

namespace MyBlog.Entities
{
    // Tüm entity sınıfları için ortak özellikleri içerir.
    public abstract class BaseEntity :IBaseEntity
    {
        // Veritabanındaki birincil anahtar.
        public int Id { get; set; }

        // Kayıt oluşturulma tarihi.
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Kayıt güncellenme tarihi.
        public DateTime? UpdatedDate { get; set; }
    }
}
