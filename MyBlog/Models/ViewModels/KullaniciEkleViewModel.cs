namespace MyBlog.Models.ViewModels
{
    public class KullaniciEkleViewModel
    {
        public string FirstName { get; set; } // Kullanıcı adı
        public string LastName { get; set; } // Kullanıcı soyadı
        public string Email { get; set; } // Kullanıcı e-posta adresi
        public string Password { get; set; } // Kullanıcı şifresi
        public string ConfirmPassword { get; set; } // Şifre tekrar doğrulama
        public List<string> Roles { get; set; } // Kullanıcının rollerini seçmesi için liste
    }
}
