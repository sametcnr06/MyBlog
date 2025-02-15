namespace MyBlog.Models.ViewModels
{
    public class RolAtamaViewModel
    {
        public string UserId { get; set; } // Kullanıcının kimlik bilgisi
        public string UserName { get; set; } // Kullanıcının kullanıcı adı
        public string FirstName { get; set; } // Kullanıcı Adı
        public string LastName { get; set; } // Kullanıcı Soyadı
        public List<string> AssignedRoles { get; set; } = new List<string>(); // Kullanıcıya atanmış roller
        public List<string> AvailableRoles { get; set; } = new List<string>(); // Sistemdeki tüm roller
    }
}
