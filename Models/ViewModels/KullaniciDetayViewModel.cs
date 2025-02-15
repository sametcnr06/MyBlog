using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Models.ViewModels
{
    public class KullaniciDetayViewModel
    {
        public string Id { get; set; } // Kullanıcının kimlik bilgisi
        public string FirstName { get; set; } // Ad
        public string LastName { get; set; } // Soyad
        public string Email { get; set; } // E-posta
        public DateTime? CreatedDate { get; set; } // Kayıt Tarihi
        public DateTime? UpdatedDate { get; set; } // Güncelleme Tarihi
        public DateTime? DeletedDate { get; set; } // Silinme Tarihi
        public bool IsDeleted { get; set; } // Silinme durumu
        public string ProfilePhotoPath { get; set; } // Profil resmi yolu
        public List<string> Roles { get; set; } = new List<string>(); // Kullanıcının roller listesi
    }
}
