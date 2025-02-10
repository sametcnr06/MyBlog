namespace MyBlog.Models.ViewModels
{
    public class KullaniciDuzenleViewModel
    {
        public string Id { get; set; } // Kullanıcı ID'si
        public string FirstName { get; set; } // Kullanıcı adı
        public string LastName { get; set; } // Kullanıcı soyadı
        public string Email { get; set; } // Kullanıcı e-posta
        public string UserName { get; set; } // Kullanıcı adı
        public bool IsActive { get; set; } // Kullanıcı aktif mi?

        public string? ProfilePhotoPath { get; set; } // Profil fotoğrafı yolu
        public IFormFile? ProfilePhoto { get; set; } // Yüklenen profil fotoğrafı dosyası

        public string? NewPassword { get; set; } // Yeni şifre (isteğe bağlı)
        public string? ConfirmPassword { get; set; } // Yeni şifre tekrar (isteğe bağlı)
    }
}
