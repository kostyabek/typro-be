/*----- 30-01-2023 -----*/
-- Create users table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
CREATE TABLE Users (
    Id int IDENTITY(1, 1) NOT NULL,
    Email varchar(255) NOT NULL UNIQUE,
    PasswordHash varchar(255) NOT NULL
)
GO