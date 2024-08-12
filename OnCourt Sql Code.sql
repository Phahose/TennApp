CREATE DATABASE Oncourt

-- DROP TABLE UserMedia
-- DROP TABLE Users

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(256) UNIQUE NOT NULL,
	UserPassword NVARCHAR(256) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    DateOfBirth DATE,
    PreferredShot NVARCHAR(100),
    SportLevel NVARCHAR(50),
    Sport NVARCHAR(50),
	Bio NVARCHAR(250),
	Gender VARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE UserMedia (
    MediaId INT PRIMARY KEY IDENTITY (1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    FilePath NVARCHAR(500) NOT NULL,
    MediaType NVARCHAR(50) NOT NULL, -- E.g., 'image', 'video'
    UploadedAt DATETIME DEFAULT GETDATE()
);


CREATE TABLE Messages (
    MessageId INT PRIMARY KEY IDENTITY (1,1),
    SenderId INT FOREIGN KEY REFERENCES Users(UserId),
    ReceiverId INT FOREIGN KEY REFERENCES Users(UserId),
    Content NVARCHAR(MAX) NOT NULL,
    SentAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Connections (
    ConnectionId INT PRIMARY KEY IDENTITY (1,1),
    UserId1 INT FOREIGN KEY REFERENCES Users(UserId),
    UserId2 INT FOREIGN KEY REFERENCES Users(UserId),
    ConnectedAt DATETIME DEFAULT GETDATE()
);



CREATE TABLE Sports (
    SportId INT PRIMARY KEY IDENTITY (1,1),
    SportName NVARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE FileUploads (
    FileId INT PRIMARY KEY IDENTITY (1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    FileName NVARCHAR(255) NOT NULL,
    FileType NVARCHAR(50) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    UploadedAt DATETIME DEFAULT GETDATE()
);



  DBCC CHECKIDENT ('Users', RESEED, 0);
  DBCC CHECKIDENT ('UserMedia', RESEED, 0);

CREATE PROCEDURE SignUp
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(256),
    @Password NVARCHAR(256), -- Plain password; hashed in the application
    @PasswordHash NVARCHAR(256), -- Hashed password
    @Sport NVARCHAR(50),
    @ProfilePhotoFilePath NVARCHAR(500) -- Path to the profile photo
AS
BEGIN
    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Insert the new user
        INSERT INTO Users (Email, UserPassword, PasswordHash, FirstName, LastName, Sport)
     
        VALUES (@Email, @Password, @PasswordHash, @FirstName, @LastName, @Sport);

        -- Get the newly inserted UserId
        DECLARE @UserId INT;
        SELECT @UserId = SCOPE_IDENTITY();

        -- Insert the profile photo into UserMedia
        INSERT INTO UserMedia (UserId, FilePath, MediaType)
        VALUES (@UserId, @ProfilePhotoFilePath, 'image');

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback the transaction if there is an error
        ROLLBACK TRANSACTION;
        
        -- Rethrow the error
        THROW;
    END CATCH
END;


CREATE PROCEDURE GetAllUsers
AS
BEGIN
    -- Select all users from the Users table
    SELECT 
        UserId,
        Email,
        FirstName,
        LastName,
        PhoneNumber,
        DateOfBirth,
        PreferredShot,
        SportLevel,
        Sport,
        Bio,
        Gender,
        CreatedAt
    FROM 
        Users;
END;

CREATE PROCEDURE GetOneUser
   @Email NVARCHAR(256)
AS
BEGIN
    -- Select all users from the Users table
    SELECT 
        *
    FROM 
        Users
    WHERE Email = @Email

END;





CREATE PROCEDURE GetUserWithMedia
    @UserId INT
AS
BEGIN
    -- Start the transaction (optional if you want to ensure atomicity)
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Retrieve user information
        SELECT 
            u.UserId,
            u.Email,
            u.FirstName,
            u.LastName,
            u.PhoneNumber,
            u.DateOfBirth,
            u.PreferredShot,
            u.SportLevel,
            u.Sport,
            u.Bio,
            u.Gender,
            u.CreatedAt
        FROM 
            Users u
        WHERE 
            u.UserId = @UserId;

        -- Retrieve user media
        SELECT 
            um.MediaId,
            um.FilePath,
            um.MediaType,
            um.UploadedAt
        FROM 
            UserMedia um
        WHERE 
            um.UserId = @UserId;

        -- Commit the transaction (optional if you started one)
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of error
        ROLLBACK TRANSACTION;

        -- Rethrow the error
        THROW;
    END CATCH
END;


CREATE PROCEDURE GetConversation
    @UserId1 INT,
    @UserId2 INT
AS
BEGIN
    -- Retrieve messages exchanged between UserId1 and UserId2
    SELECT 
        m.MessageId,
        m.SenderId,
        m.ReceiverId,
        m.Content,
        m.SentAt
    FROM 
        Messages m
    WHERE 
        (m.SenderId = @UserId1 AND m.ReceiverId = @UserId2)
        OR (m.SenderId = @UserId2 AND m.ReceiverId = @UserId1)
    ORDER BY 
        m.SentAt;
END;
