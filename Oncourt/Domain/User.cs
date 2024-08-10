namespace OnCourt.Domain
{
    public class User
    {
        private int UserId { get; set; }
        private string FirstName { get; set; } = string.Empty;
        private string LastName { get; set; } = string.Empty;
        private string Email { get; set; } = string.Empty;
        private string Password { get; set; }= string.Empty;
        private string PasswordSalt { get; set; } = string.Empty;
        private DateTime DateOfBirth { get; set; }
        private string PreferredShot { get; set; } = string.Empty;
        private string SportLevel { get; set; } = string.Empty;
        private string Sport {  get; set; } = string.Empty;
        private string Bio { get; set; } = string.Empty;    
        private string Gender { get; set; } = string.Empty;
        private DateTime CreatedAt { get; set; }
    }
}
