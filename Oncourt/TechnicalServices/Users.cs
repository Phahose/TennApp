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
                            UserId = GetOneUserReader["UserId"] != DBNull.Value ? (int)GetOneUserReader["UserId"] : 0,
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

        public List<UserMedia> GetUserMedia(int userId)
        {
            List<UserMedia> userMediaList = new();
            using (SqlConnection onCourtConnection = new SqlConnection(connectionString))
            {
                onCourtConnection.Open();

                try
                {
                    SqlCommand getUserMediaCommand = new SqlCommand()
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "GetUserMedia",
                        Connection = onCourtConnection,
                    };

                    SqlParameter userIdParameter = new SqlParameter()
                    {
                        ParameterName = "@UserId",
                        SqlDbType = SqlDbType.Int,
                        SqlValue = userId,
                        Direction = ParameterDirection.Input,
                    };

                    getUserMediaCommand.Parameters.Add(userIdParameter);

                    using (SqlDataReader reader = getUserMediaCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userMediaList.Add(new UserMedia()
                            {
                                MediaId = reader["MediaId"] != DBNull.Value ? (int)reader["MediaId"] : 0,
                                UserId = reader["UserId"] != DBNull.Value ? (int)reader["UserId"] : 0,
                                FilePath = reader["FilePath"] != DBNull.Value ? (string)reader["FilePath"] : string.Empty,
                                MediaType = reader["MediaType"] != DBNull.Value ? (string)reader["MediaType"] : string.Empty,
                                CreatedDate = reader["UploadedAt"] != DBNull.Value ? (DateTime)reader["UploadedAt"] : default(DateTime)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle exception as needed
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return userMediaList;
        }


        public bool UpdateUser(User user)
        {
            bool confirmation = true;
            using (SqlConnection onCourtConnection = new SqlConnection(connectionString))
            {
                onCourtConnection.Open();

                try
                {
                    using (SqlCommand updateUserCommand = new SqlCommand("UpdateUser", onCourtConnection))
                    {
                        updateUserCommand.CommandType = CommandType.StoredProcedure;

                        SqlParameter UserIdParameter = new SqlParameter
                        {
                            ParameterName = "@UserId",
                            SqlDbType = SqlDbType.Int,
                            SqlValue = user.UserId,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter FirstNameParameter = new SqlParameter
                        {
                            ParameterName = "@FirstName",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.FirstName,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter LastNameParameter = new SqlParameter
                        {
                            ParameterName = "@LastName",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.LastName,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter EmailParameter = new SqlParameter
                        {
                            ParameterName = "@Email",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.Email,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter UserPasswordParameter = new SqlParameter
                        {
                            ParameterName = "@UserPassword",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.Password, // Assuming you want to update the password as well
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter PasswordHashParameter = new SqlParameter
                        {
                            ParameterName = "@PasswordHash",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.PasswordSalt,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter PhoneNumberParameter = new SqlParameter
                        {
                            ParameterName = "@PhoneNumber",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.PhoneNumber,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter DateOfBirthParameter = new SqlParameter
                        {
                            ParameterName = "@DateOfBirth",
                            SqlDbType = SqlDbType.DateTime,
                            SqlValue = (object)user.DateOfBirth ?? (object)DBNull.Value, // Handle null values
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter PreferredShotParameter = new SqlParameter
                        {
                            ParameterName = "@PreferredShot",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.PreferredShot,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter SportLevelParameter = new SqlParameter
                        {
                            ParameterName = "@SportLevel",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.SportLevel,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter SportParameter = new SqlParameter
                        {
                            ParameterName = "@Sport",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.Sport,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter BioParameter = new SqlParameter
                        {
                            ParameterName = "@Bio",
                            SqlDbType = SqlDbType.NVarChar,
                            SqlValue = user.Bio,
                            Direction = ParameterDirection.Input
                        };

                        SqlParameter GenderParameter = new SqlParameter
                        {
                            ParameterName = "@Gender",
                            SqlDbType = SqlDbType.VarChar,
                            SqlValue = user.Gender,
                            Direction = ParameterDirection.Input
                        };

                        updateUserCommand.Parameters.Add(UserIdParameter);
                        updateUserCommand.Parameters.Add(FirstNameParameter);
                        updateUserCommand.Parameters.Add(LastNameParameter);
                        updateUserCommand.Parameters.Add(EmailParameter);
                        updateUserCommand.Parameters.Add(UserPasswordParameter);
                        updateUserCommand.Parameters.Add(PasswordHashParameter);
                        updateUserCommand.Parameters.Add(PhoneNumberParameter);
                        updateUserCommand.Parameters.Add(DateOfBirthParameter);
                        updateUserCommand.Parameters.Add(PreferredShotParameter);
                        updateUserCommand.Parameters.Add(SportLevelParameter);
                        updateUserCommand.Parameters.Add(SportParameter);
                        updateUserCommand.Parameters.Add(BioParameter);
                        updateUserCommand.Parameters.Add(GenderParameter);

                        updateUserCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    confirmation = false;
                    throw;
                }
            }
            return confirmation;
        }


        private static byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            // Hash the password with PBKDF2 using HMACSHA256
            return new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256).GetBytes(32);
        }

    }
}
