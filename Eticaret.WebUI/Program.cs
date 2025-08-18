using Eticaret.Data;
using Eticaret.WebUI.Services; // (CartService için)
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Eticaret.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DatabaseContext>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
            {
                x.LoginPath = "/Account/SignIn";
                x.AccessDeniedPath = "/AccessDenied";
                x.Cookie.Name= "Account";
                x.Cookie.MaxAge=TimeSpan.FromDays(5);  /*oturum açýnca ne kadar süre kapanmayacaðýný bu gösteriyo */
                x.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminPolicy",policy=>policy.RequireClaim(ClaimTypes.Role,"Admin"));
                x.AddPolicy("UserPolicy",policy=>policy.RequireClaim(ClaimTypes.Role,"Admin","User","Customer"));
            });

            // Session ve Sepet servisi için eklemeler
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".Eticaret.Session";
                options.IdleTimeout = TimeSpan.FromHours(4);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICartService, CartService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // EKLEDÝM

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "admin",
                pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
