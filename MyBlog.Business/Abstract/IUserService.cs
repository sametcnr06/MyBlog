using MyBlog.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    public interface IUserService
    {
        // ID'ye göre kullanıcıyı getir.
        Task<ApplicationUser> GetUserByIdAsync(int id);

        // Kullanıcı adını kullanarak kullanıcıyı getir.
        Task<ApplicationUser> GetUserByUsernameAsync(string username);

        // Tüm kullanıcıları getir.
        Task<List<ApplicationUser>> GetAllUsersAsync();

        // Yeni bir kullanıcı oluştur ve parola ayarla.
        Task<bool> CreateUserAsync(ApplicationUser user, string password);

        // Kullanıcı bilgilerini güncelle.
        Task<bool> UpdateUserAsync(ApplicationUser user);

        // Kullanıcıyı sil.
        Task<bool> DeleteUserAsync(int id);
    }
}
