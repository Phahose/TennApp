use TenApp

Create Table Users(
 [UserID] int Not Null, 
 [FirstName] varchar(255) Not Null,
 [LastName] varchar (255) Not Null,
 [Email] varchar(255) Not Null,
 [Bio] varchar (200) Null,
 [Image] varbinary(max),
 [ImageDescription] varchar(60) Null,
 [SkillLevel] varchar(25) ,
 [NormalizedEmail] [nvarchar](256) NULL,
 [EmailConfirmed] [bit] NOT NULL,
 [PasswordHash] [nvarchar](max) NULL,
 [SecurityStamp] [nvarchar](max) NULL,
 [ConcurrencyStamp] [nvarchar](max) NULL,
 [PhoneNumber] [nvarchar](max) NULL,
 [PhoneNumberConfirmed] [bit] NOT NULL,
 [TwoFactorEnabled] [bit] NOT NULL,
 [LockoutEnd] [datetimeoffset](7) NULL,
 [LockoutEnabled] [bit] NOT NULL,
 [AccessFailedCount] [int] NOT NULL,
 
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
 (
	[UserID] ASC
 )
)

-- Drop Table Users


Create Table Courts (
 CourtName varchar (255),
 SurfaceType NVARCHAR(50),
 NumberOfCourts INT,
 LightingAvailable BIT,
 FacilityName NVARCHAR(255),
 Reservable BIT,
 AvailabilityInfo NVARCHAR(MAX),
 MaintenanceStatus NVARCHAR(MAX),
 Fees DECIMAL(10, 2),
 BookingInfo NVARCHAR(MAX),
 CourtOwnership NVARCHAR(50),
 Amenities NVARCHAR(MAX),
 AccessibilityInfo NVARCHAR(MAX),
 ReviewsRatings NVARCHAR(MAX),
 HistoricalData NVARCHAR(MAX),
 MaintenanceHistory NVARCHAR(MAX),
 SecurityInfo NVARCHAR(MAX),
 CourtImage VARBINARY(MAX),
 CourtImageDescription NVARCHAR(MAX)


 CONSTRAINT [PK_Courts] PRIMARY KEY CLUSTERED
 (
	CourtName ASC
 )
)

CREATE TABLE SkippedPlayers (
    [SkippedPlayerID] INT PRIMARY KEY,
	[SkippedPlayerFirstName] varchar(255),
	[SkippedPlayerLastName] varchar(255),
	[SkippedPlayerLevel] varchar(25) ,
    [UserID] INT,
    

    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE AcceptedPlayers (
    [AcceptedPlayerID] INT PRIMARY KEY,
	[AcceptedPlayerFirstName] varchar(255),
	[AcceptedPlayerLastName] varchar(255),
	[AcceptedPlayerLevel] varchar(25) ,
    [UserID] INT,
    

    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);