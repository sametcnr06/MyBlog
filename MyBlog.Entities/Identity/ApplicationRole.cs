using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MyBlog.Entities.Identity
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid().ToString(); // Benzersiz bir Id oluştur.
            Users = new List<ApplicationUserRole>(); // Kullanıcı ilişkisi için koleksiyon başlatılır.
        }

        public string? Description { get; set; } // Rol açıklaması (isteğe bağlı).
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Rolün oluşturulma tarihi.
        public bool IsActive { get; set; } = true; // Rolün aktif olup olmadığını belirtir.
        public ICollection<ApplicationUserRole> Users { get; set; } // Roldeki kullanıcılar.
    }
}
