using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.Identity;
using MyBlog.Entities;
using Microsoft.AspNetCore.Identity;

namespace MyBlog.DataAccess.Contexts
{
    public class MyBlogContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
        {
        }

        // Blog yazılarını temsil eden tablo.
        public DbSet<Post> Posts { get; set; }

        // Kategorileri temsil eden tablo.
        public DbSet<Category> Categories { get; set; }

        // Yorumları temsil eden tablo.
        public DbSet<Comment> Comments { get; set; }

        // Etiketleri temsil eden tablo.
        public DbSet<Tag> Tags { get; set; }

        // Post ve Tag ilişkisini temsil eden tablo.
        public DbSet<PostTag> PostTags { get; set; }

        // Bildirimleri temsil eden tablo.
        public DbSet<Notification> Notifications { get; set; }

        // Kullanıcı takiplerini temsil eden tablo.
        public DbSet<Favorite> Favorites { get; set; }

        // Roller tablosu (Identity ile entegre).
        public DbSet<ApplicationRole> Roles { get; set; } // Uygulamada kullanılan rollerin tutulduğu tablo.

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Identity tablolarını varsayılan olarak yapılandır.

            builder.HasDefaultSchema("blog"); // Tüm tablolar için varsayılan şemayı 'blog' olarak ayarla.

            // **Yorumlar ve Blog Yazıları İlişkisi**
            builder.Entity<Comment>()
                .HasOne(c => c.Post) // Her yorum bir blog yazısına aittir.
                .WithMany(p => p.Comments) // Bir blog yazısı birden fazla yoruma sahip olabilir.
                .HasForeignKey(c => c.PostId) // PostId üzerinden ilişki kurulur.
                .OnDelete(DeleteBehavior.Restrict); // Silme işlemi sırasında kısıtlama uygula.

            // **Yorumlar ve Kullanıcılar İlişkisi**
            builder.Entity<Comment>()
                .HasOne(c => c.User) // Her yorum bir kullanıcıya aittir.
                .WithMany(u => u.Comments) // Bir kullanıcı birden fazla yorum yapabilir.
                .HasForeignKey(c => c.UserId) // UserId üzerinden ilişki kurulur.
                .OnDelete(DeleteBehavior.Restrict); // Silme işlemi sırasında kısıtlama uygula.

            // **PostTag İlişkisi**
            builder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId }); // PostTag tablosunda birincil anahtar olarak composite key tanımlanır.

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Post) // Her PostTag bir blog yazısına aittir.
                .WithMany(p => p.PostTags) // Bir blog yazısı birden fazla PostTag'e sahip olabilir.
                .HasForeignKey(pt => pt.PostId) // PostId üzerinden ilişki kurulur.
                .OnDelete(DeleteBehavior.Cascade); // Post silindiğinde ilgili PostTag'ler silinir.

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Tag) // Her PostTag bir etikete aittir.
                .WithMany(t => t.PostTags) // Bir etiket birden fazla PostTag'e sahip olabilir.
                .HasForeignKey(pt => pt.TagId) // TagId üzerinden ilişki kurulur.
                .OnDelete(DeleteBehavior.Cascade); // Tag silindiğinde ilgili PostTag'ler silinir.

            // **Favorite İlişkisi**
            builder.Entity<Favorite>()
                .HasOne(f => f.Follower) // Favoriyi ekleyen kullanıcı.
                .WithMany(u => u.FollowedFavorites) // Bir kullanıcı birden fazla favori oluşturabilir.
                .HasForeignKey(f => f.FollowerId) // FollowerId üzerinden ilişki kurulur.
                .OnDelete(DeleteBehavior.Restrict); // Silme işlemi sırasında kısıtlama uygula.

            builder.Entity<Favorite>()
                .HasOne(f => f.FollowedUser) // Favoriye alınan kullanıcı.
                .WithMany(u => u.FollowerFavorites) // Bir kullanıcı birden fazla kişi tarafından favoriye alınabilir.
                .HasForeignKey(f => f.FollowedUserId) // FollowedUserId üzerinden ilişki kurulur.
                .OnDelete(DeleteBehavior.Restrict); // Silme işlemi sırasında kısıtlama uygula.

            // **Bildirimler ve Kullanıcılar İlişkisi**
            builder.Entity<Notification>()
                .HasOne(n => n.User) // Her bildirim bir kullanıcıya aittir.
                .WithMany(u => u.Notifications) // Bir kullanıcı birden fazla bildirim alabilir.
                .HasForeignKey(n => n.UserId) // UserId üzerinden ilişki kurulur.
                .OnDelete(DeleteBehavior.Restrict); // Silme işlemi sırasında kısıtlama uygula.

            // **ApplicationRole ve ApplicationUserRole İlişkisi**
            builder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User) // Kullanıcı bilgisi
                .WithMany(u => u.UserRoles) // Kullanıcının rollerle ilişkisi
                .HasForeignKey(ur => ur.UserId) // UserId üzerinden ilişki
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde ilişkili roller silinir.

            builder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role) // Rol bilgisi
                .WithMany(r => r.Users) // Rolün kullanıcılarla ilişkisi
                .HasForeignKey(ur => ur.RoleId) // RoleId üzerinden ilişki
                .OnDelete(DeleteBehavior.Cascade); // Rol silindiğinde ilişkili kullanıcılar silinir.

            // **IdentityUserRole<string> İçin Composite Key**
            // Identity framework içinde kullanılan UserRole tablosu için birincil anahtar tanımlanır.
            builder.Entity<IdentityUserRole<string>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite key tanımlandı.


        }
    }
}
