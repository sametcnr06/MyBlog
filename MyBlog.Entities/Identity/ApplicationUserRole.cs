using Microsoft.AspNetCore.Identity;

namespace MyBlog.Entities.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; } // Kullanıcı bilgisi
        public virtual ApplicationRole Role { get; set; } // Rol bilgisi
    }
}
