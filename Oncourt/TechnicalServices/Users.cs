using Microsoft.Data.SqlClient;
using OnCourt.Domain;
using System.Data;
using System.Security.Cryptography;

namespace OnCourt.TechnicalServices
{
    public class Users
    {
        private string? connectionString;
        public Users() 
        {

            ConfigurationBuilder DatabaseUserBuilder = new ConfigurationBuilder();
            DatabaseUserBuilder.SetBasePath(Directory.GetCurrentDirectory());
            DatabaseUserBuilder.AddJsonFile("appsettings.json");
            IConfiguration DatabaseUserConfiguration = DatabaseUserBuilder.Build();
            connectionString = DatabaseUserConfiguration.GetConnectionString("OnCourt");
        }


        public bool AddUser(User NewUser, UserMedia userMedia)
        {
            bool confirmation = true;
            SqlConnection onCourtConnection = new();
            onCourtConnection.ConnectionString = connectionString;
            onCourtConnection.Open();

            try
            {
                // Generate a new salt
                byte[] salt = new byte[128 / 8]; // 16 bytes for salt
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // Hash the password with the salt
                byte[] hashedPassword = HashPasswordWithSalt(NewUser.Password, salt);

                // Convert the salt and hashed password to Base64 for storage
                string saltBase64 = Convert.ToBase64String(salt);
                string hashedPasswordBase64 = Convert.ToBase64String(hashedPassword);


                SqlCommand AddUserCommand = new()
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = onCourtConnection,
                    CommandText = "SignUp",
                };

                SqlParameter FirstNameParameter = new()
                {
                    ParameterName = "@FirstName",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = NewUser.FirstName,
                    Direction = ParameterDirection.Input,
                };

                SqlParameter LastNameParameter = new()
                {
                    ParameterName = "@LastName",
                    SqlValue = NewUser.LastName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                };

                SqlParameter EmailParameter = new()
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = NewUser.Email,
                    Direction = ParameterDirection.Input,
                };

                SqlParameter PasswordParameter = new()
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = hashedPasswordBase64,
                    Direction = ParameterDirection.Input,
                };

                SqlParameter PasswordHashParameter = new()
                {
                    ParameterName = "@PasswordHash",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = saltBase64,
                    Direction = ParameterDirection.Input,
                };

                SqlParameter SportParameter = new()
                {
                    ParameterName = "@Sport",
                    SqlValue = NewUser.Sport,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                };

                SqlParameter ProfilePhotoFilePathParameter = new()
                {
                    ParameterName = "@ProfilePhotoFilePath",
                    SqlValue = userMedia.FilePath,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                };

                AddUserCommand.Parameters.Add(FirstNameParameter);
                AddUserCommand.Parameters.Add(LastNameParameter);
                AddUserCommand.Parameters.Add(EmailParameter);
                AddUserCommand.Parameters.Add(PasswordParameter);
                AddUserCommand.Parameters.Add (PasswordHashParameter);
                AddUserCommand.Parameters.Add(SportParameter);
                AddUserCommand.Parameters.Add(ProfilePhotoFilePathParameter);

                AddUserCommand.ExecuteNonQuery();
                onCourtConnection.Close();

            }
            catch (Exception)
            {

                confirmation = false;
                throw;
            }
            return  confirmation;
        }

        public User GetOneUser(string email)
        {
            User aUser = new();
            SqlConnection onCourtConnection = new();
            onCourtConnection.ConnectionString = connectionString;
            onCourtConnection.Open();

            try
            {
                SqlCommand GetUserCommand = new()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetOneUser",
                    Connection = onCourtConnection,
                };

                SqlParameter EmailParameter = new()
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = email,
                    Direction = ParameterDirection.Input,
                };

                GetUserCommand.Parameters.Add(EmailParameter);

                SqlDataReader GetOneUserReader = GetUserCommand.ExecuteReader();
                while (GetOneUserReader.Read())
                {
                    if (GetOneUserReader.HasRows)
                    {
                        aUser = new User()
                        {
                            FirstName = GetOneUserReader["FirstName"] != DBNull.Value ? (string)GetOneUserReader["FirstName"] : string.Empty,
                            LastName = GetOneUserReader["LastName"] != DBNull.Value ? (string)GetOneUserReader["LastName"] : string.Empty,
                            Email = GetOneUserReader["Email"] != DBNull.Value ? (string)GetOneUserReader["Email"] : string.Empty,
                            Password = GetOneUserReader["UserPassword"] != DBNull.Value ? (string)GetOneUserReader["UserPassword"] : string.Empty,
                            PasswordSalt = GetOneUserReader["PasswordHash"] != DBNull.Value ? (string)GetOneUserReader["PasswordHash"] : string.Empty,
                            // Adjust type casting and default value for PhoneNumber if it's nullable
                            PhoneNumber = GetOneUserReader["PhoneNumber"] != DBNull.Value ? (int)GetOneUserReader["PhoneNumber"] : 0,
                            DateOfBirth = GetOneUserReader["DateOfBirth"] != DBNull.Value ? (DateTime)GetOneUserReader["DateOfBirth"] : default(DateTime),
                            PreferredShot = GetOneUserReader["PreferredShot"] != DBNull.Value ? (string)GetOneUserReader["PreferredShot"] : string.Empty,
                            SportLevel = GetOneUserReader["SportLevel"] != DBNull.Value ? (string)GetOneUserReader["SportLevel"] : string.Empty,
                            Sport = GetOneUserReader["Sport"] != DBNull.Value ? (string)GetOneUserReader["Sport"] : string.Empty,
                            Bio = GetOneUserReader["Bio"] != DBNull.Value ? (string)GetOneUserReader["Bio"] : string.Empty,
                            Gender = GetOneUserReader["Gender"] != DBNull.Value ? (string)GetOneUserReader["Gender"] : string.Empty,
                            // Handle CreatedAt as nullable or provide default value
                            CreatedAt = GetOneUserReader["CreatedAt"] != DBNull.Value ? (DateTime)GetOneUserReader["CreatedAt"] : default(DateTime)

                        };
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            
            return aUser;
        }

        private static byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            // Hash the password with PBKDF2 using HMACSHA256
            return new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256).GetBytes(32);
        }

    }
}
