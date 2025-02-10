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
    // Veritabaný baðlantýsýný appsettings.json üzerinden al
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogConnection"));
});

// Identity ayarlarý
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Parola kurallarýný özelleþtirme
    options.Password.RequireDigit = true; // Parolada rakam gereksinimi
    options.Password.RequiredLength = 8; // Minimum parola uzunluðu
    options.Password.RequireUppercase = true; // Büyük harf gereksinimi
    options.Password.RequireNonAlphanumeric = true; // Alfasayýsal olmayan karakter gereksinimi
})
.AddEntityFrameworkStores<MyBlogContext>() // Identity'nin DbContext ile entegre edilmesi
.AddDefaultTokenProviders(); // Varsayýlan token saðlayýcýlar

// Dependency Injection (DI) ayarlarý
builder.Services.AddScoped<IUserService, UserManager>(); // IUserService ile UserManager eþleþtirilmesi
builder.Services.AddScoped<IRoleService, RoleManager>(); // IRoleService ile RoleManager eþleþtirilmesi
builder.Services.AddScoped<IPostService, PostManager>(); // IPostService ile PostManager eþleþtirilmesi
builder.Services.AddScoped<ICategoryService, CategoryManager>(); // ICategoryService ile CategoryManager eþleþtirilmesi
builder.Services.AddScoped<ITagService, TagManager>(); // ITagService ile TagManager eþleþtirilmesi

// MVC hizmetlerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware pipeline ayarlarý
if (!app.Environment.IsDevelopment())
{
    // Üretim ortamý için hata sayfasý
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

app.UseHttpsRedirection(); // HTTPS yönlendirmesi
app.UseStaticFiles(); // Statik dosyalara izin ver
app.UseRouting(); // Yönlendirme middleware'i
app.UseAuthentication(); // Kimlik doðrulama middleware'i
app.UseAuthorization(); // Yetkilendirme middleware'i

// Default route ayarý
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Admin Paneli route'u
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

// Kullanýcý Listesi route'u
app.MapControllerRoute(
    name: "admin-kullanici-listesi",
    pattern: "Admin/KullaniciListesi",
    defaults: new { controller = "Admin", action = "KullaniciListesi" });

// Roller ve kullanýcýlar seed iþlemi
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    // Rol yönetimi
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    string[] roleNames = { "Admin", "Editor", "Writer", "Subscriber" }; // Gerekli roller
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName)) // Rol varsa eklemez
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName }); // Rol oluþtur
        }
    }

    // Kullanýcý yönetimi
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Admin kullanýcýyý seed et    
    if (await userManager.FindByEmailAsync("samet@blog.com") == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "samet@blog.com",
            Email = "samet@blog.com",
            FirstName = "Samet",
            LastName = "Çýnar",
            PhotoPath = "default.jpg", // Varsayýlan profil resmi
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(adminUser, "Samet123*");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin"); // Admin rolü ata
        }
    }

    // Editor kullanýcýyý seed et
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
            await userManager.AddToRoleAsync(editorUser, "Editor"); // Editor rolü ata
        }
    }

    // Writer kullanýcýyý seed et
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
            await userManager.AddToRoleAsync(writerUser, "Writer"); // Writer rolü ata
        }
    }
}

app.Run(); // Uygulamayý çalýþtýr
