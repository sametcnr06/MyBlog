using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.Business.Concrete;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities.Identity;

var builder = WebApplication.CreateBuilder(args);

// DbContext ayarlarý
builder.Services.AddDbContext<MyBlogContext>(options =>
{
    // Veritabaný baðlantýsýný appsettings.json'dan al
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogConnection"));
});

// Identity ayarlarý
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Parola kurallarýný özelleþtirme
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<MyBlogContext>()
.AddDefaultTokenProviders();

// Service (Manager) katmanlarýnýn DI ayarlarý
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IRoleService, RoleManager>();
builder.Services.AddScoped<IPostService, PostManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ITagService, TagManager>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<IFavoriteService, FavoriteManager>();
builder.Services.AddScoped<INotificationService, NotificationManager>();

// MVC hizmetlerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Production dýþýndaki ortamlarda özel hata sayfasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS yönlendirme
app.UseHttpsRedirection();

// Statik dosyalar
app.UseStaticFiles();

// Routing
app.UseRouting();

// Kimlik doðrulama & yetkilendirme
app.UseAuthentication();
app.UseAuthorization();

// Varsayýlan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Admin Paneli rota örneði
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

// Kullanýcý Listesi rota örneði
app.MapControllerRoute(
    name: "admin-kullanici-listesi",
    pattern: "Admin/KullaniciListesi",
    defaults: new { controller = "Admin", action = "KullaniciListesi" });

app.MapControllerRoute(
    name: "writer_routes",
    pattern: "Writer/{controller=Writer}/{action=Index}/{id?}"
);

// Varsayýlan kullanýcý ve rollerin seed edilmesi
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    // Rol yönetimi
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    string[] roleNames = { "Admin", "Editor", "Writer", "Subscriber" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }
    }

    // Kullanýcý yönetimi
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Admin kullanýcý oluþturma
    if (await userManager.FindByEmailAsync("samet@blog.com") == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "samet@blog.com",
            Email = "samet@blog.com",
            FirstName = "Samet",
            LastName = "Çýnar",
            PhotoPath = "default.jpg",
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(adminUser, "Samet123*");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Editor kullanýcý
    if (await userManager.FindByEmailAsync("enver@myblog.com") == null)
    {
        var editorUser = new ApplicationUser
        {
            UserName = "enver@myblog.com",
            Email = "enver@myblog.com",
            FirstName = "Enver",
            LastName = "Alioðlu",
            PhotoPath = "default.jpg",
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(editorUser, "Enver123*");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(editorUser, "Editor");
        }
    }

    // Writer kullanýcý
    if (await userManager.FindByEmailAsync("bengu@myblog.com") == null)
    {
        var writerUser = new ApplicationUser
        {
            UserName = "bengu@myblog.com",
            Email = "bengu@myblog.com",
            FirstName = "Bengü Su",
            LastName = "Akay",
            PhotoPath = "default.jpg",
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(writerUser, "Bengusu123*");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(writerUser, "Writer");
        }
    }
}

app.Run();
