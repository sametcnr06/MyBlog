using MyBlog.DataAccess.Abstract;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities.Identity;

namespace MyBlog.DataAccess.Concorte
{
    public class EfUserDal : EfEntityRepositoryBase<ApplicationUser, MyBlogContext>, IUserDal
    {
        public EfUserDal(MyBlogContext context) : base(context)
        {
            // MyBlogContext üzerinden DbContext işlemlerini yönet.
        }
    }
}
