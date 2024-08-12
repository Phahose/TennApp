#nullable disable
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnCourt.Domain;
using OnCourt.TechnicalServices;
using System.Security.Claims;
using System.Security.Cryptography;

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
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Users usersController = new Users();
            if (Submit == "Join")
            {
                // Test if the Email Is Already Used
                User testUser = usersController.GetOneUser(Email);

                if (testUser.FirstName == "")
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
                            usersController.AddUser(NewUser, UserMedia);

                            // Redirect or return success
                            SuccessMessage = "Sign Up Successfull You can Now Proceed to Login";
                            return RedirectToPage("/Index");
                        }
                        catch (Exception ex)
                        {
                            // Handle exceptions, e.g., log the error
                            return StatusCode(500, "Internal server error: " + ex.Message);
                        }
                    }
                    else
                    {
                        // IF there is No Image Provided   
                        ErrorMessage = "Sign Up Failed ~ A profile Picture is Mandatory";
                        return Page();
                    }
                    
                }
                else
                {
                    ErrorMessage = "Sign Up Failed ~ This Email is Alredy Used";
                    return Page();
                }
            }
            else if (Submit == "Enter")
            {
                User loginUser = usersController.GetOneUser(Email);

                // Convert DB Data Back to byte[] form because they werr stored in the DB as strings
                byte[] salt = Convert.FromBase64String(loginUser.PasswordSalt);
                byte[] storedHashedpassword = Convert.FromBase64String(loginUser.Password);

                // Convert user Input to byte[] hash and then to strings
                byte[] enteredHashedPassword = HashPasswordWithSalt(Password, salt);
                string enteredHashedPasswordBase64 = Convert.ToBase64String(enteredHashedPassword);

                Password = enteredHashedPasswordBase64;

                string UserEmail = loginUser.Email;
                //string UserRole = existingUser.Role;
                string UserPassword = loginUser.Password;
                string UserSalt = loginUser.PasswordSalt;

                if (Email == UserEmail)
                {
                    // Compare the Two arrays and not the Hashed Figures
                    if (ByteArraysAreEqual(storedHashedpassword, enteredHashedPassword))
                    {
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, Email),
                            };
                        var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authProperties = new AuthenticationProperties
                        {
                            #region
                            //AllowRefresh = <bool>,
                            // Refreshing the authentication session should be allowed.
                            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                            // The time at which the authentication ticket expires. A
                            // value set here overrides the ExpireTimeSpan option of
                            // CookieAuthenticationOptions set with AddCookie.
                            //IsPersistent = true,
                            // Whether the authentication session is persisted across
                            // multiple requests. When used with cookies, controls
                            // whether the cookie's lifetime is absolute (matching the
                            // lifetime of the authentication ticket) or session‐based.
                            //IssuedUtc = <DateTimeOffset>,
                            // The time at which the authentication ticket was issued.
                            //RedirectUri = <string>
                            // The full path or absolute URI to be used as an http
                            // redirect response value.
                            #endregion
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);
                        SuccessMessage = "Login Success";
                        HttpContext.Session.SetString("Email", Email);

                        return RedirectToPage("/Profile");
                        

                    }
                }

                ErrorMessage = "Login Failed Invalid Email Or Passoword";
                return Page();


            }

             return BadRequest("Invalid submit action.");           
        }







        // Hashes the Enterd password with the same hashing algorithm that was used to Hash them into the DB
        private static byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            // Hash the password with PBKDF2 using HMACSHA256
            return new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256).GetBytes(32);
        }

        private bool ByteArraysAreEqual(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
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
