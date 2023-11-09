namespace Xnept
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddRazorPages();
            var app = builder.Build();

            //Configure the HTTP reqest pipeline
            app.UseStaticFiles(); // add for wwroot
            app.UseRouting();

            app.MapRazorPages();

            app.Run();
        }
    }
}