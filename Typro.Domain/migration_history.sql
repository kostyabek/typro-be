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
INSERT INTO Roles(Name)
VALUES ('admin');
INSERT INTO Roles(Name)
VALUES ('user');

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

-- Add WordsModeTypes, TimeModeTypes and TrainingConfigurations tables
IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'WordsModeTypes'
                and xtype = 'U')
CREATE TABLE WordsModeTypes
(
    Id            int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    NumberOfWords int                             NOT NULL,
    CONSTRAINT UNIQ_NumberOfWords UNIQUE (NumberOfWords)
)
GO

INSERT INTO WordsModeTypes(NumberOfWords)
VALUES (0);
INSERT INTO WordsModeTypes(NumberOfWords)
VALUES (10);
INSERT INTO WordsModeTypes(NumberOfWords)
VALUES (25);
INSERT INTO WordsModeTypes(NumberOfWords)
VALUES (50);
INSERT INTO WordsModeTypes(NumberOfWords)
VALUES (100);

IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'TimeModeTypes'
                and xtype = 'U')
CREATE TABLE TimeModeTypes
(
    Id              int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    NumberOfSeconds int                             NOT NULL,
    CONSTRAINT UNIQ_NumberOfSeconds UNIQUE (NumberOfSeconds)
)
GO

INSERT INTO TimeModeTypes(NumberOfSeconds)
VALUES (0);
INSERT INTO TimeModeTypes(NumberOfSeconds)
VALUES (15);
INSERT INTO TimeModeTypes(NumberOfSeconds)
VALUES (30);
INSERT INTO TimeModeTypes(NumberOfSeconds)
VALUES (60);
INSERT INTO TimeModeTypes(NumberOfSeconds)
VALUES (120);

IF NOT EXISTS(SELECT *
              FROM sysobjects
              WHERE name = 'TrainingConfigurations'
                and xtype = 'U')
CREATE TABLE TrainingConfigurations
(
    Id                   int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    IsPunctuationEnabled bit                             NOT NULL,
    AreNumbersEnabled    bit                             NOT NULL,
    WordsModeTypeId      int                             NOT NULL,
    TimeModeTypeId       int                             NOT NULL,
    CONSTRAINT FK_WordsModeTypes_WordsModeTypeId_Id FOREIGN KEY (WordsModeTypeId) REFERENCES WordsModeTypes (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_TimeModeTypes_TimeModeTypeId_Id FOREIGN KEY (TimeModeTypeId) REFERENCES TimeModeTypes (Id) ON DELETE NO ACTION
)
GO