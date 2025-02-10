using MyBlog.Business.Abstract;
using MyBlog.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager; // ASP.NET Identity'nin UserManager sınıfı.

        public UserManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager; // Constructor üzerinden UserManager nesnesini al.
        }

        public async Task<ApplicationUser> GetUserByIdAsync(int id)
        {
            // ID'ye göre kullanıcıyı getir.
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            // Kullanıcı adına göre kullanıcıyı getir.
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            // Tüm kullanıcıları liste olarak getir.
            return _userManager.Users.ToList();
        }

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
        {
            // Yeni bir kullanıcı oluştur ve parola ayarla.
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded; // Başarılı olup olmadığını döndür.
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            // Kullanıcı bilgilerini güncelle.
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded; // Başarılı olup olmadığını döndür.
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            // ID'ye göre kullanıcıyı sil.
            var user = await _userManager.FindByIdAsync(id.ToString()); // Kullanıcıyı bul.
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user); // Kullanıcıyı sil.
                return result.Succeeded; // Başarılı olup olmadığını döndür.
            }
            return false; // Kullanıcı bulunamazsa false döndür.
        }
    }
}
