using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.Entities.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager; // Kullanıcı işlemleri için UserManager eklendi

        public RoleManager(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager; // Dependency Injection
            _userManager = userManager; // UserManager Dependency Injection
        }

        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles
                        .Include(r => r.Users) // Kullanıcıları dahil et
                        .ThenInclude(ur => ur.User) // Kullanıcı bilgilerini dahil et
                        .ToListAsync(); // Rolleri listele
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await RoleExistsAsync(roleName)) // Eğer rol zaten varsa
                return false;

            var result = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName); // Belirtilen rol mevcut mu?
        }

        // Kullanıcıları belirli bir role göre filtrele
        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName); // Rolü bul
            if (role == null)
                return new List<ApplicationUser>(); // Rol bulunamazsa boş bir liste dön

            // Kullanıcıları role göre filtrele
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.ToList();
        }
    }
}
