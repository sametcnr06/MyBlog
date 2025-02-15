using MyBlog.Core.Dtos;
using MyBlog.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    // Rolleri yönetmek için gerekli servis arayüzü
    public interface IRoleService
    {
        Task<List<ApplicationRole>> GetAllRolesAsync(); // Tüm roller ve kullanıcıları getir
        Task<bool> CreateRoleAsync(string roleName); // Yeni rol oluştur
        Task<bool> DeleteRoleAsync(string roleName); // Rolü sil
        Task<bool> RoleExistsAsync(string roleName); // Rolün varlığını kontrol et
        // Kullanıcıları belirli bir role göre filtrele
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName); // Rol adına göre kullanıcıları getir

        List<UserRoleDto> GetList();

        //Task<bool> CreateRoleUserAsync(string roleUser);
    }
}
