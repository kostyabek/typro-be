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
