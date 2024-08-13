#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnCourt.Domain;
using OnCourt.TechnicalServices;

namespace OnCourt.Pages
{
    public class PlayersModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; } = string.Empty;
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
