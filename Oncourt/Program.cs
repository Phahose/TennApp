using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace nekwom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddRazorPages();
            builder.Services.AddSession();

            builder.Services.AddControllers(); // ? Add support for API controllers

            builder.Services.AddRazorPages().AddRazorPagesOptions(o =>
            {
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Index";
            });

            builder.Services.AddAuthorization();

            // ? Enable CORS for React Native requests
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactNative",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseStaticFiles();
            app.UseRouting();

            // ? Enable CORS
            app.UseCors("AllowReactNative");

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            // ? Map API Controllers (so React Native can call them)
            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}
