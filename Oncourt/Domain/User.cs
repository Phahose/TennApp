﻿namespace OnCourt.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }= string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string PreferredShot { get; set; } = string.Empty;
        public string SportLevel { get; set; } = string.Empty;
        public string Sport {  get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;    
        public string Gender { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
