using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sedatodev1.Data;
using sedatodev1.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DATABASE
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -------------------- IDENTITY
builder.Services.AddIdentity<Uye, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3; // "sau" için 3 yeterli
})
.AddEntityFrameworkStores<UygulamaDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// -------------------- PIPELINE -----------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// -------------------- ROUTING -----------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// -------------------- ADMIN OLUÞTURMA ----------------
async Task CreateAdminUser(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<Uye>>();

    // Roller yoksa oluþtur
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("Uye"))
        await roleManager.CreateAsync(new IdentityRole("Uye"));

    // Admin bilgileri
    string adminEmail = "G231210383@sakarya.edu.tr";
    string adminPassword = "sau";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new Uye
        {
            UserName = adminEmail,
            Email = adminEmail,
            AdSoyad = "Sakarya Admin"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// Admin oluþturmayý çalýþtýr
using (var scope = app.Services.CreateScope())
{
    await CreateAdminUser(scope.ServiceProvider);
}

app.Run();
