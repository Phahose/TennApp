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
            var app = builder.Build();

            //Configure the HTTP reqest pipeline
            app.UseStaticFiles(); // add for wwroot
            app.UseRouting();
            app.UseSession();
            app.MapRazorPages();
            app.UseAuthentication();
            app.UseAuthorization();
            app.Run();
        }
    }
}