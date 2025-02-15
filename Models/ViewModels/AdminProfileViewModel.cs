namespace MyBlog.Models.ViewModels
{
    public class AdminProfileViewModel
    {
        public string UserId { get; set; } // Kullanıcı ID
        public string UserName { get; set; } // Kullanıcı adı
        public string Email { get; set; } // E-posta adresi
        public string FirstName { get; set; } // İsim
        public string LastName { get; set; } // Soyisim
        public string PhotoPath { get; set; } // Profil Fotoğrafı Yolu
        public string Role { get; set; } // Rol
    }
}