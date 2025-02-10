using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.DataAccess.Contexts
{
    public class MyBlogContextFactory : IDesignTimeDbContextFactory<MyBlogContext>
    {
        public MyBlogContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MyBlogContext>();

            // Bağlantı dizgisi çevresel değişken veya appsettings üzerinden alınabilir
            var connectionString = Environment.GetEnvironmentVariable("MYBLOG_DB_CONNECTION") ??
                                   "Data Source=Samet\\SQLEXPRESS;Initial Catalog=MyBlogDb;Integrated Security=True;TrustServerCertificate=True;";

            builder.UseSqlServer(connectionString);

            return new MyBlogContext(builder.Options);
        }
    }
}
