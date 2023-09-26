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

CREATE TABLE VideoFlashcardsSchema.Decks(
    DeckId INT IDENTITY(1, 1),
    UserId INT,
    DeckName NVARCHAR(50),
    DeckCreated DATETIME,
    DeckUpdated DATETIME
);
GO

CREATE TABLE VideoFlashcardsSchema.Flashcards(
    FlashcardId INT IDENTITY(1, 1),
    DeckId INT,
    UserId INT,
    FlashcardUrl NVARCHAR(255),
    FlashcardTitle NVARCHAR(50),
    FlashcardDescription NVARCHAR(255),
    FlashcardCreated DATETIME,
    FlashcardUpdated DATETIME
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

-- Decks
-- EXEC VideoFlashcardsSchema.spDecks_Get
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spDecks_Get
    @UserId INT = NULL,
    @DeckId INT = NULL
AS
BEGIN
    SELECT * FROM VideoFlashcardsSchema.Decks As Decks
        WHERE Decks.UserId = ISNULL(@UserId, Decks.UserId)
        And Decks.DeckId = ISNULL(@DeckId, Decks.DeckId)
END
GO

-- EXEC VideoFlashcardsSchema.spDecks_Upsert @UserId = 1, @DeckName = 'Apple Deck', @DeckId = 1
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spDecks_Upsert
    @UserId INT,
    @DeckName NVARCHAR(50),
    @DeckId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM VideoFlashcardsSchema.Decks WHERE DeckId = @DeckId)
        BEGIN
            INSERT INTO VideoFlashcardsSchema.Decks(
                UserId,
                DeckName,
                DeckCreated,
                DeckUpdated
            ) VALUES (
                @UserId,
                @DeckName,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE VideoFlashcardsSchema.Decks
                SET DeckName = @DeckName,
                    DeckUpdated = GETDATE()
                WHERE DeckId = @DeckId
                    AND UserId = @UserId 
        END
END
GO

-- EXEC VideoFlashcardsSchema.spDecks_Delete @DeckId = 2, @UserId = 1
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spDecks_Delete
    @DeckId INT
    , @UserId INT 
AS
BEGIN
    DELETE FROM VideoFlashcardsSchema.Decks
        WHERE DeckId = @DeckId
            AND UserId = @UserId
END
GO

--Flashcards
-- EXEC VideoFlashcardsSchema.spFlashcards_Get
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spFlashcards_Get
    @UserId INT = NULL,
    @DeckId INT = NULL,
    @FlashcardId INT = NULL
AS
BEGIN
    SELECT * FROM VideoFlashcardsSchema.Flashcards
        WHERE UserId = ISNULL(@UserId, UserId)
        AND DeckId = ISNULL(@DeckId, DeckId)
        AND FlashcardId = ISNULL(@FlashcardId, FlashcardId)
END
GO

-- EXEC VideoFlashcardsSchema.spFlashcards_Upsert @UserId = 1, @DeckId = 1, @FlashcardUrl = 'testing url', @FlashcardTitle = 'testing title', @FlashcardDescription = 'testing description', @FlashcardId = 1
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spFlashcards_Upsert
    @UserId INT,
    @DeckId INT,
    @FlashcardUrl NVARCHAR(255),
    @FlashcardTitle NVARCHAR(50),
    @FlashcardDescription NVARCHAR(255),
    @FlashcardId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM VideoFlashcardsSchema.Flashcards WHERE FlashcardId = @FlashcardId)
        BEGIN
            INSERT INTO VideoFlashcardsSchema.Flashcards(
                UserId,
                DeckId,
                FlashcardUrl,
                FlashcardTitle,
                FlashcardDescription,
                FlashcardCreated,
                FlashcardUpdated
            ) VALUES (
                @UserId,
                @DeckId,
                @FlashcardUrl,
                @FlashcardTitle,
                @FlashcardDescription,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE VideoFlashcardsSchema.Flashcards
            SET FlashcardUrl = @FlashcardUrl,
                FlashcardTitle = @FlashcardTitle,
                FlashcardDescription = @FlashcardDescription,
                FlashcardUpdated = GETDATE()
            WHERE FlashcardId = @FlashcardId
        END
END
GO

-- EXEC VideoFlashcardsSchema.spFlashcards_Delete @UserId = 1, @FlashcardId = 1
CREATE OR ALTER PROCEDURE VideoFlashcardsSchema.spFlashcards_Delete
    @UserId INT,
    @FlashcardId INT
AS
BEGIN
    DELETE FROM VideoFlashcardsSchema.Flashcards
        WHERE UserId = @UserId
            AND FlashcardId = @FlashcardId
END
