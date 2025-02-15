using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.Core.Dtos;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities.Identity;

namespace MyBlog.Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager; // Kullanıcı işlemleri için UserManager eklendid
        private readonly MyBlogContext _context; // Kullanıcı işlemleri için UserManager eklendid

        public RoleManager(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, MyBlogContext context)
        {
            _roleManager = roleManager; // Dependency Injection
            _userManager = userManager; // UserManager Dependency Injection
            _context = context;
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

        public List<UserRoleDto> GetList()
        {
            var result = _context.UserRoles
    .GroupBy(x => new { x.RoleId })
    .Select(group => new UserRoleDto
    {
        Name = _context.Roles.FirstOrDefault(r => r.Id == group.Key.RoleId).Name, // Role tablosundan ismi al
        UserCount = group.Count(),
        //UserRoles = group.ToList() // ApplicationUserRole nesnelerini de döndür
    })
    .ToList();

            return result;
        }

        //public Task<bool> CreateRoleUserAsync(ApplicationRole roleUser)
        //{
        //    if ( RoleExistsAsync(roleName)) // Eğer rol zaten varsa
        //        return false;

        //    var result =  _roleManager.CreateAsync(new ApplicationUserRole {  RoleId = roleUser.Id, UserId = roleUser.Id });
        //    return result.Succeeded;
        //}
    }
}
