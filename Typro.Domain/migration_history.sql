/*----- 30-01-2023 -----*/
-- Create Users table
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'Users'
                and xtype = 'U')
CREATE TABLE Users
(
    Id           int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    Email        varchar(255)                    NOT NULL UNIQUE,
    PasswordHash varchar(255)                    NOT NULL
)
GO

/*----- 11-02-2023 -----*/
-- Add Roles table
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'Roles'
                and xtype = 'U')
CREATE TABLE Roles
(
    Id   int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    Name varchar(255)                    NOT NULL UNIQUE,
    CHECK (Name IN ('admin', 'user'))
)
GO

-- Insert roles
IF NOT EXISTS(SELECT *
              FROM dbo.Roles
              WHERE Name IN ('admin', 'user'))
    BEGIN
        INSERT INTO Roles(Name)
        VALUES ('admin');
        INSERT INTO Roles(Name)
        VALUES ('user');
    END


-- Add Role column to Users table
ALTER TABLE Users
    ADD RoleId int NOT NULL
        CONSTRAINT FK_Roles_RoleId_Id FOREIGN KEY (RoleId) REFERENCES Roles (Id) ON DELETE NO ACTION
GO

-- Add RefreshTokens table
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'RefreshTokens'
                and xtype = 'U')
CREATE TABLE RefreshTokens
(
    Id             int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    UserId         int                             NOT NULL,
    Token          varchar(255)                    NOT NULL,
    CreatedDate    datetime2                       NOT NULL,
    ExpirationDate datetime2                       NOT NULL,
    IsRevoked      bit                             NOT NULL DEFAULT 0,
    CONSTRAINT FK_Users_UserId_Id FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE,
    CONSTRAINT UNIQ_UserId_Token UNIQUE (UserId, Token)
)
GO

/*----- 04-03-2023 -----*/
-- Add SupportedLanguages table
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'SupportedLanguages'
                and xtype = 'U')
CREATE TABLE SupportedLanguages
(
    Id   int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    Name varchar(50)                     NOT NULL,
    CONSTRAINT UNIQ_Name UNIQUE (Name)
)
GO

-- Populate languages
IF NOT EXISTS(SELECT *
              FROM dbo.SupportedLanguages
              WHERE Name IN ('English'))
    INSERT INTO SupportedLanguages(Name)
    VALUES ('English');
IF NOT EXISTS(SELECT *
              FROM dbo.SupportedLanguages
              WHERE Name IN ('Ukrainian'))
    INSERT INTO SupportedLanguages(Name)
    VALUES ('Ukrainian');

-- Add TrainingConfigurations table
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'TrainingConfigurations'
                and xtype = 'U')
CREATE TABLE TrainingConfigurations
(
    Id                   int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    IsPunctuationEnabled bit                             NOT NULL DEFAULT 0,
    AreNumbersEnabled    bit                             NOT NULL DEFAULT 0,
    WordsModeType        int                             NOT NULL DEFAULT 25 CHECK (WordsModeType IN (0, 10, 25, 50, 100)),
    TimeModeType         int                             NOT NULL DEFAULT 0 CHECK (TimeModeType IN (0, 15, 30, 60, 120)),
    LanguageId           int                             NOT NULL DEFAULT 1,
    CONSTRAINT FK_TrainingConfigurations_LanguageId_Id FOREIGN KEY (LanguageId) REFERENCES SupportedLanguages (Id) ON DELETE NO ACTION,
)
GO

-- Add training configuration FK to Users
ALTER TABLE Users
    ADD TrainingConfigurationId int NOT NULL
        CONSTRAINT FK_TrainingConfigurations_TrainingConfigurationId_Id FOREIGN KEY (TrainingConfigurationId) REFERENCES TrainingConfigurations (Id) ON DELETE NO ACTION
GO

/*----- 14-03-2023 -----*/
-- Add Words table
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'Words'
                and xtype = 'U')
CREATE TABLE Words
(
    Id         int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    Name       nvarchar(255)                   NOT NULL,
    LanguageId int                             NOT NULL
        CONSTRAINT FK_Words_LanguageId_Id FOREIGN KEY (LanguageId) REFERENCES SupportedLanguages (Id) ON DELETE NO ACTION,
)
GO

/*----- 28-03-2023 -----*/
-- Add TrainingResults table
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'TrainingResults'
                and xtype = 'U')
CREATE TABLE TrainingResults
(
    Id                 int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    UserId             int                             NOT NULL,
    WordsPerMinute     float                           NOT NULL,
    Accuracy           float                           NOT NULL,
    TimeInMilliseconds int                             NOT NULL,
    WordsModeType      int                             NOT NULL DEFAULT 25 CHECK (WordsModeType IN (0, 10, 25, 50, 100)),
    TimeModeType       int                             NOT NULL DEFAULT 0 CHECK (TimeModeType IN (0, 15, 30, 60, 120)),
    DateConducted      datetime2                       NOT NULL,
    LanguageId         int                             NOT NULL,
    CorrectLetters     int                             NOT NULL,
    IncorrectLetters   int                             NOT NULL,
    ExtraLetters       int                             NOT NULL,
    InitialLetters     int                             NOT NULL
        CONSTRAINT FK_Users_UserId_Id FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE,
    CONSTRAINT FK_SupportedLanguages_LanguageId_Id FOREIGN KEY (LanguageId) REFERENCES SupportedLanguages (Id) ON DELETE NO ACTION,
)
GO

/*----- 08-04-2023 -----*/
-- Add user data to Users table
ALTER TABLE Users
    ADD
        CreatedDate datetime2 NOT NULL,
        Nickname nvarchar(20) NOT NULL
GO

/*----- 09-04-2023 -----*/
-- Add stored procedures for profile data
CREATE PROCEDURE dbo.ProfileHighLevelInfo @UserId int
AS
SELECT u1.Nickname             as Nickname,
       u1.CreatedDate          as MemberSince,
       (SELECT count(*)
        FROM dbo.TrainingResults tr
                 join dbo.Users u2 ON tr.UserId = u2.Id
        WHERE u2.Id = @UserId) as TestsStarted,
       (SELECT count(*)
        FROM dbo.TrainingResults tr
                 join dbo.Users u3 ON tr.UserId = u3.Id
        WHERE tr.WordsPerMinute <> -1
          AND u3.Id = @UserId) as TestsCompleted
FROM dbo.Users u1
WHERE u1.Id = @UserId;
GO

CREATE PROCEDURE dbo.BestResults @UserId int
AS
SELECT temp.WordsModeType,
       temp.TimeModeType,
       temp.WordsPerMinute,
       temp.Accuracy,
       temp.DateConducted
FROM (SELECT ROW_NUMBER() OVER (PARTITION BY tr.WordsModeType, tr.TimeModeType ORDER BY tr.WordsPerMinute desc) AS RowNumber,
             tr.WordsModeType,
             tr.TimeModeType,
             tr.WordsPerMinute,
             tr.Accuracy,
             tr.DateConducted,
             tr.UserId
      FROM dbo.TrainingResults tr
               join dbo.Users u ON tr.UserId = u.Id
      WHERE tr.WordsPerMinute <> -1) as temp
WHERE temp.RowNumber = 1
  AND temp.UserId = @UserId
GO

/*----- 10-04-2023 -----*/
-- Add stored procedure for leaderboards
CREATE PROCEDURE dbo.Leaderboard @TimeModeType int,
                                 @WordsModeType int,
                                 @LanguageId int,
                                 @FromDate datetime2,
                                 @ToDate datetime2
AS
SELECT ROW_NUMBER() OVER (ORDER BY temp.WordsPerMinute DESC) AS Place,
       temp.Nickname,
       temp.WordsPerMinute,
       temp.Accuracy,
       temp.DateConducted
FROM (SELECT ROW_NUMBER() OVER (PARTITION BY u.Nickname ORDER BY tr.WordsPerMinute DESC) AS LocalPlace,
             u.Nickname                                                                  AS Nickname,
             tr.WordsPerMinute                                                           AS WordsPerMinute,
             tr.Accuracy                                                                 AS Accuracy,
             tr.DateConducted                                                            AS DateConducted
      FROM dbo.TrainingResults tr
               JOIN dbo.Users u ON tr.UserId = u.Id
      WHERE tr.TimeModeType = @TimeModeType
        AND tr.WordsModeType = @WordsModeType
        AND tr.LanguageId = @LanguageId
        AND tr.DateConducted >= @FromDate
        AND tr.DateConducted < DATEADD(DAY, 1, @ToDate)
        AND tr.WordsPerMinute > 0) temp
WHERE temp.LocalPlace = 1
GO
