#nullable disable
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnCourt.Domain;
using OnCourt.TechnicalServices;

namespace nekwom.Pages
{
    public class IndexModel : PageModel
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
        public IFormFile Image1 { get; set; }
        [BindProperty]
        public string Submit { get; set; } = string.Empty;
        public User NewUser { get; set; }
        public UserMedia UserMedia {  get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Users users = new Users();
            if (Submit == "Join")
            {
                if (Image1 != null)
                {
                    try
                    {
                        // Upload the profile image and get the file path
                        var filePath = await UploadProfileImage(Image1);

                        // Create NewUser and UserMedia
                        NewUser = new User
                        {
                            FirstName = FirstName,
                            LastName = LastName,
                            Email = Email,
                            Password = Password,
                            Sport = Sport,
                        };

                        UserMedia = new UserMedia
                        {
                            FilePath = filePath
                        };

                        // Add user and media to the database
                        users.AddUser(NewUser, UserMedia);

                        // Redirect or return success
                        return RedirectToPage("/Index");
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, e.g., log the error
                        return StatusCode(500, "Internal server error: " + ex.Message);
                    }
                }

                return BadRequest("No image provided.");
            }

            return BadRequest("Invalid submit action.");
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

                // Store file path and other details in the database
                // Example code: SaveFilePathToDatabase(fileName);

                return $"/uploads/{fileName}";
            }
            throw new InvalidOperationException("No file uploaded.");
        }


    }
}
