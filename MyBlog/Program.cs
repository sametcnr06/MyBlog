using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.Business.Concrete;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities.Identity;

var builder = WebApplication.CreateBuilder(args);

// DbContext ayarlar�
builder.Services.AddDbContext<MyBlogContext>(options =>
{
    // Veritaban� ba�lant�s�n� appsettings.json �zerinden al
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogConnection"));
});

// Identity ayarlar�
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Parola kurallar�n� �zelle�tirme
    options.Password.RequireDigit = true; // Parolada rakam gereksinimi
    options.Password.RequiredLength = 8; // Minimum parola uzunlu�u
    options.Password.RequireUppercase = true; // B�y�k harf gereksinimi
    options.Password.RequireNonAlphanumeric = true; // Alfasay�sal olmayan karakter gereksinimi
})
.AddEntityFrameworkStores<MyBlogContext>() // Identity'nin DbContext ile entegre edilmesi
.AddDefaultTokenProviders(); // Varsay�lan token sa�lay�c�lar

// Dependency Injection (DI) ayarlar�
builder.Services.AddScoped<IUserService, UserManager>(); // IUserService ile UserManager e�le�tirilmesi
builder.Services.AddScoped<IRoleService, RoleManager>(); // IRoleService ile RoleManager e�le�tirilmesi
builder.Services.AddScoped<IPostService, PostManager>(); // IPostService ile PostManager e�le�tirilmesi
builder.Services.AddScoped<ICategoryService, CategoryManager>(); // ICategoryService ile CategoryManager e�le�tirilmesi
builder.Services.AddScoped<ITagService, TagManager>(); // ITagService ile TagManager e�le�tirilmesi

// MVC hizmetlerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware pipeline ayarlar�
if (!app.Environment.IsDevelopment())
{
    // �retim ortam� i�in hata sayfas�
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

app.UseHttpsRedirection(); // HTTPS y�nlendirmesi
app.UseStaticFiles(); // Statik dosyalara izin ver
app.UseRouting(); // Y�nlendirme middleware'i
app.UseAuthentication(); // Kimlik do�rulama middleware'i
app.UseAuthorization(); // Yetkilendirme middleware'i

// Default route ayar�
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Admin Paneli route'u
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

// Kullan�c� Listesi route'u
app.MapControllerRoute(
    name: "admin-kullanici-listesi",
    pattern: "Admin/KullaniciListesi",
    defaults: new { controller = "Admin", action = "KullaniciListesi" });

// Roller ve kullan�c�lar seed i�lemi
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    // Rol y�netimi
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    string[] roleNames = { "Admin", "Editor", "Writer", "Subscriber" }; // Gerekli roller
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName)) // Rol varsa eklemez
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName }); // Rol olu�tur
        }
    }

    // Kullan�c� y�netimi
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Admin kullan�c�y� seed et    
    if (await userManager.FindByEmailAsync("samet@blog.com") == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "samet@blog.com",
            Email = "samet@blog.com",
            FirstName = "Samet",
            LastName = "��nar",
            PhotoPath = "default.jpg", // Varsay�lan profil resmi
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(adminUser, "Samet123*");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin"); // Admin rol� ata
        }
    }

    // Editor kullan�c�y� seed et
    if (await userManager.FindByEmailAsync("enver@myblog.com") == null)
    {
        var editorUser = new ApplicationUser
        {
            UserName = "enver@myblog.com",
            Email = "enver@myblog.com",
            FirstName = "Enver",
            LastName = "Alio�lu",
            PhotoPath = "default.jpg",
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(editorUser, "Enver123*");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(editorUser, "Editor"); // Editor rol� ata
        }
    }

    // Writer kullan�c�y� seed et
    if (await userManager.FindByEmailAsync("bengu@myblog.com") == null)
    {
        var writerUser = new ApplicationUser
        {
            UserName = "bengu@myblog.com",
            Email = "bengu@myblog.com",
            FirstName = "Beng� Su",
            LastName = "Akay",
            PhotoPath = "default.jpg",
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(writerUser, "Bengusu123*");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(writerUser, "Writer"); // Writer rol� ata
        }
    }
}

app.Run(); // Uygulamay� �al��t�r
