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
        FOREIGN KEY (RoleId) REFERENCES Roles (Id)