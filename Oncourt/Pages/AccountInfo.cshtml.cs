#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnCourt.Domain;
using OnCourt.TechnicalServices;

namespace OnCourt.Pages
{
    public class AccountInfoModel : PageModel
    {
        [BindProperty]
        public string FirstName { get; set; } = string.Empty;
        [BindProperty]
        public string LastName { get; set; } = string.Empty;
        [BindProperty]
        public string Sport { get; set; } = string.Empty;
        [BindProperty]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;
        [BindProperty]
        public string Submit { get; set; } = string.Empty;
        [BindProperty]
        public IFormFile Image1 { get; set; }
        [BindProperty]
        public IFormFile Image2 { get; set; }
        [BindProperty]
        public IFormFile Image3 { get; set; }
        [BindProperty]
        public IFormFile Image4 { get; set; }
        [BindProperty]
        public IFormFile Image5 { get; set; }
        [BindProperty]
        public IFormFile Image6 { get; set; }
        public User NewUser { get; set; }
        public List<UserMedia> UserMedia { get; set; }

        public void OnGet()
        {
            Users usersController = new Users();
            Email = HttpContext.Session.GetString("Email");

            NewUser = usersController.GetOneUser(Email);
            UserMedia = usersController.GetUserMedia(NewUser.UserId);
        }
    }
}
