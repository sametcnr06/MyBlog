namespace MyBlog.Models.ViewModels
{
    // View için kullanılan model (rollerin bilgilerini taşır)
    public class RoleViewModel
    {
        public string Name { get; set; } // Rol adı
        public int UserCount { get; set; } // Roldeki kullanıcı sayısı
    }
}
