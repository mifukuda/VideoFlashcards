CREATE SCHEMA VideoFlashcardsSchema;
GO

CREATE TABLE VideoFlashcardsSchema.Users
(
    UserId INT IDENTITY(1, 1) PRIMARY KEY,
    Email NVARCHAR(50),
    Firstname NVARCHAR(50),
    LastName NVARCHAR(50)
);
GO

CREATE TABLE TutorialAppSchema.Auth(
	Email NVARCHAR(50) PRIMARY KEY,
	PasswordHash VARBINARY(MAX),
	PasswordSalt VARBINARY(MAX)
);
GO

-- USERS
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spUsers_Get
    @UserId INT = NULL
AS
BEGIN
    SELECT * FROM VideoFlashcardsSchema.Users AS Users
        WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
END
GO

CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spUsers_Upsert
    @Email NVARCHAR(50),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @UserId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM VideoFlashcardsSchema.Users WHERE UserId = @UserId)
        BEGIN
        IF NOT EXISTS (SELECT * FROM VideoFlashcardsSchema.Users WHERE Email = @Email)
            BEGIN
                INSERT INTO VideoFlashcardsSchema.Users(
                    [Users].[Email],
                    [Users].[FirstName],
                    [Users].[LastName] 
                ) VALUES (
                    @Email,
                    @FirstName,
                    @LastName
                )
            END
        END
    ELSE
        BEGIN
            UPDATE VideoFlashcardsSchema.Users
                SET Email = @Email,
                    FirstName = @FirstName,
                    LastName = @LastName
                WHERE UserId = @UserId
                
        END
END
GO

CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spUsers_Delete
    @UserId INT
AS
BEGIN
    DELETE FROM VideoFlashcardsSchema.Users
        WHERE UserId = @UserId
END
GO

-- AUTH
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spRegistration_Upsert
    @Email NVARCHAR(50),
    @PasswordHash VARBINARY(MAX),
    @PasswordSalt VARBINARY(MAX)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM VideoFlashcardsSchema.Auth WHERE Email = @Email)
        BEGIN
            INSERT INTO VideoFlashcardsSchema.Auth(
                [Email],
                [PasswordHash],
                [PasswordSalt]
            ) VALUES (
                @Email,
                @PasswordHash,
                @PasswordSalt
            )
        END
    ELSE
        BEGIN
            UPDATE VideoFlashcardsSchema.Auth
                SET PasswordHash = @PasswordHash,
                    PasswordSalt = @PasswordSalt
                WHERE Email = @Email
        END
END
GO

