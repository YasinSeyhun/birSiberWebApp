using Microsoft.EntityFrameworkCore;
using BirSiberDanismanlik.Data;
using BirSiberDanismanlik.Services;
using Microsoft.AspNetCore.Identity;
using BirSiberDanismanlik.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Add Google OAuth2
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var configuration = builder.Configuration;
        var googleClientId = configuration["Authentication:Google:ClientId"]!;
        var googleClientSecret = configuration["Authentication:Google:ClientSecret"]!;
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.Always;
});

// Add Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SmsService>();
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
});
builder.Services.AddHostedService<ReminderBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Seed Roles and Example Instructors
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string[] roles = { "Admin", "User", "Çalışan" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    // Remove all existing 'Çalışan' users before seeding
    var existingInstructors = await userManager.GetUsersInRoleAsync("Çalışan");
    foreach (var user in existingInstructors)
    {
        await userManager.RemoveFromRoleAsync(user, "Çalışan");
        await userManager.DeleteAsync(user);
    }
    // Seed example instructors (real names, proper casing, and 3 test instructors)
    var egitmenler = new[] {
        new ApplicationUser { 
            UserName = "ahmethamdiatalay",
            FullName = "Ahmet Hamdi Atalay",
            Email = "ahmethamdi.atalay@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "besimaltinok",
            FullName = "Besim Altınok",
            Email = "besim.altinok@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "mesutkalyoncu",
            FullName = "Mesut Kalyoncu",
            Email = "mesut.kalyoncu@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "tufanvolkankarabacak",
            FullName = "Tufan Volkan Karabacak",
            Email = "tufanvolkan.karabacak@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "enesaslanbakan",
            FullName = "Enes Aslanbakan",
            Email = "enes.aslanbakan@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "burakdayioglu",
            FullName = "Burak Dayıoğlu",
            Email = "burak.dayioglu@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "enestayboga",
            FullName = "Enes Tayboğa",
            Email = "enes.tayboga@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "alperkandemir",
            FullName = "Alper Kandemir",
            Email = "alper.kandemir@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        },
        new ApplicationUser { 
            UserName = "yasinseyhun",
            FullName = "Yasin Seyhun",
            Email = "yasin.seyhun@egitmen.com", 
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        }
    };

    foreach (var egitmen in egitmenler)
    {
        var user = await userManager.FindByEmailAsync(egitmen.Email);
        if (user == null)
        {
            var result = await userManager.CreateAsync(egitmen, "Egitmen123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(egitmen, "Çalışan");
                Console.WriteLine($"Created instructor: {egitmen.FullName}");
            }
            else
            {
                Console.WriteLine($"Failed to create instructor {egitmen.FullName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(user, "Çalışan"))
            {
                await userManager.AddToRoleAsync(user, "Çalışan");
                Console.WriteLine($"Added Çalışan role to existing user: {user.FullName}");
            }
            else
            {
                Console.WriteLine($"User already exists and is a Çalışan: {user.FullName}");
            }
        }
    }
    // Seed admin user
    var adminEmail = "admin@admin.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new ApplicationUser { UserName = "admin", Email = adminEmail, CreatedAt = DateTime.UtcNow };
        var result = await userManager.CreateAsync(admin, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

app.Run();
