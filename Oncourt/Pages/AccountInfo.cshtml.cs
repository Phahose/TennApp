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
        public string SportLevel { get; set; } = string.Empty;
        [BindProperty]
        public string PreferedShot { get; set; } = string.Empty;
        [BindProperty]
        public int PhoneNumber { get; set; }
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
        public List<string> ErrorList { get; set; }
        public List<string> FilePaths { get; set; }

        public void OnGet()
        {
            Users usersController = new Users();
            Email = HttpContext.Session.GetString("Email");

            NewUser = usersController.GetOneUser(Email);
            UserMedia = usersController.GetUserMedia(NewUser.UserId);
        }

        public async Task<IActionResult> OnPostAsync()
        {          
            Users usersController;

            if (FirstName == "")
            {
                ErrorList.Add("FirstName is Required");
            }

            if (LastName == "")
            {
                ErrorList.Add("LastName is Required");
            }

            if (Email == "")
            {
                ErrorList.Add("Email is Required");
            }

            if (Sport == "")
            {
                ErrorList.Add("Sport is Required");
            }

            if (SportLevel == "")
            {
                ErrorList.Add("Level is Required");
            }
            
            if (PreferedShot == "")
            {
                ErrorList.Add("PreferredShot is Required");
            }

            if (ErrorList.Count < 1)
            {
                NewUser = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password,
                    Sport = Sport,
                    SportLevel = SportLevel,    
                    PreferredShot = PreferedShot,
                    PhoneNumber = PhoneNumber
                };

               /* UserMedia = new UserMedia
                {
                    FilePath = filePath
                };*/
            }


            return Page();
        }

        public async Task<string> UploadProfileImage(IFormFile profileImage)
        {
            if (profileImage != null && profileImage.Length > 0)
            {
                // Generate a unique filename to avoid conflicts
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }


                return $"/uploads/{fileName}";
            }
            throw new InvalidOperationException("No file uploaded.");
        }
    }
}
