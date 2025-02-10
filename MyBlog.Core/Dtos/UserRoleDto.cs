using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Core.Dtos
{
    public class UserRoleDto
    {
        public string Name { get; set; } // Rol adı
        public int UserCount { get; set; } // Roldeki kullanıcı sayısı
    }
}
