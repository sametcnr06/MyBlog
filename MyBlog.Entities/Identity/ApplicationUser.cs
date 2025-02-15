using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MyBlog.Entities.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        // 📌 Kullanıcı Bilgileri
        public string? FirstName { get; set; } // Kullanıcının adı.
        public string? SecondName { get; set; } // Kullanıcının ikinci adı (isteğe bağlı).
        public string? LastName { get; set; } // Kullanıcının soyadı.
        public string? SecondLastName { get; set; } // Kullanıcının ikinci soyadı (isteğe bağlı).

        // 📌 Kullanıcı Profil Bilgileri
        public string? PhotoPath { get; set; } // Kullanıcının profil fotoğrafı yolu.
        public string? About { get; set; } // Kullanıcının hakkında kısmı.

        // 📌 Kullanıcı Durum Bilgileri
        public bool IsActive { get; set; } // Kullanıcı aktif mi?
        public bool IsDeleted { get; set; } // Kullanıcı silinmiş mi?

        // 📌 Tarih Bilgileri
        public DateTime CreatedDate { get; set; } // Kullanıcının oluşturulma tarihi.
        public DateTime? LastLoginDate { get; set; } // Kullanıcının son giriş tarihi.
        public DateTime? UpdatedDate { get; set; } // Kullanıcı bilgilerinin son güncellenme tarihi.
        public DateTime? DeletedDate { get; set; } // Kullanıcının silinme tarihi.

        // 📌 Kullanıcı İlişkileri
        public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Kullanıcının yaptığı yorumlar.
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>(); // Kullanıcının aldığı bildirimler.
        public ICollection<Favorite> FollowedFavorites { get; set; } = new List<Favorite>(); // Kullanıcının takip ettiği favoriler.
        public ICollection<Favorite> FollowerFavorites { get; set; } = new List<Favorite>(); // Kullanıcıyı takip edenlerin favorileri.
        public ICollection<ApplicationUserRole> UserRoles { get; set; } // Kullanıcının rollerle ilişkisi.

        // 📌 Varsayılan Değerler ve Constructor
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(); // Benzersiz bir Id oluştur.
            CreatedDate = DateTime.Now; // Varsayılan olarak oluşturulma tarihi atanır.
            IsActive = true; // Kullanıcı varsayılan olarak aktif.
            IsDeleted = false; // Kullanıcı varsayılan olarak silinmemiş.
            UserRoles = new List<ApplicationUserRole>(); // Kullanıcının roller listesi başlatılır.
        }
    }
}
