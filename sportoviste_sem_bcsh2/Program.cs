using LiteDB;
using Microsoft.AspNetCore.DataProtection;
using sportoviste_sem_bcsh2.Models;
using sportoviste_sem_bcsh2.Services;
using System.IO;

namespace sportoviste_sem_bcsh2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Pøidání služby pro session s èasovým limitem
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout po 30 minutách neaktivity
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Konfigurace Data Protection (pøed builder.Build())
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"./keys"))
                .SetApplicationName("sportoviste_sem_bcsh2");

            // Registrace LiteDbService jako singletonu
            builder.Services.AddSingleton<LiteDbService>();

            // Sestavení aplikace
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

            // Použití middlewaru pro autorizaci a session
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
