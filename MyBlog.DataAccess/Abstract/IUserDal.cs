using MyBlog.Entities.Identity;

namespace MyBlog.DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<ApplicationUser>
    {
        // Kullanıcıya özel veri erişim metotlarını burada tanımlayabilirsin.
    }
}
