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
    // Veritaban� ba�lant�s�n� appsettings.json'dan al
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogConnection"));
});

// Identity ayarlar�
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Parola kurallar�n� �zelle�tirme
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<MyBlogContext>()
.AddDefaultTokenProviders();

// Service (Manager) katmanlar�n�n DI ayarlar�
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

// Production d���ndaki ortamlarda �zel hata sayfas�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS y�nlendirme
app.UseHttpsRedirection();

// Statik dosyalar
app.UseStaticFiles();

// Routing
app.UseRouting();

// Kimlik do�rulama & yetkilendirme
app.UseAuthentication();
app.UseAuthorization();

// Varsay�lan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Admin Paneli rota �rne�i
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

// Kullan�c� Listesi rota �rne�i
app.MapControllerRoute(
    name: "admin-kullanici-listesi",
    pattern: "Admin/KullaniciListesi",
    defaults: new { controller = "Admin", action = "KullaniciListesi" });

app.MapControllerRoute(
    name: "writer_routes",
    pattern: "Writer/{controller=Writer}/{action=Index}/{id?}"
);

// Varsay�lan kullan�c� ve rollerin seed edilmesi
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    // Rol y�netimi
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    string[] roleNames = { "Admin", "Editor", "Writer", "Subscriber" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }
    }

    // Kullan�c� y�netimi
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Admin kullan�c� olu�turma
    if (await userManager.FindByEmailAsync("samet@blog.com") == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "samet@blog.com",
            Email = "samet@blog.com",
            FirstName = "Samet",
            LastName = "��nar",
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

    // Editor kullan�c�
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
            await userManager.AddToRoleAsync(editorUser, "Editor");
        }
    }

    // Writer kullan�c�
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
            await userManager.AddToRoleAsync(writerUser, "Writer");
        }
    }
}

app.Run();
