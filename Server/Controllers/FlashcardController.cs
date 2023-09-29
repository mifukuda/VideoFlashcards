using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class FlashcardsController : ControllerBase
{
    // Wrapper for Dapper methods
    private readonly DataContextDapper _dapper;

    public FlashcardsController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    // Get userId from JWT token and return all flashcards associated with deckId
    [HttpGet("{deckId}/{flashcardId}")]
    public IEnumerable<Flashcard> GetFlashcards(int deckId = 0, int flashcardId = 0)
    {
        // Stored procedure defined in CreateDatabase.sql
        string sql = "EXEC VideoFlashcardsSchema.spFlashcards_Get @UserId = @UserIdParameter";
        DynamicParameters sqlParameters = new DynamicParameters();
        // If userId == 0, deckId == 0, and flashcardId == 0, return all flashcards
        sqlParameters.Add("@UserIdParameter", User.FindFirst("userId")?.Value);
        if(deckId != 0)
        {
            sql += ", @DeckId = @DeckIdParameter";
            sqlParameters.Add("@DeckIdParameter", deckId, DbType.Int32);
        }
        if(flashcardId != 0)
        {
            sql += ", @FlashcardId = @FlashcardIdParameter";
            sqlParameters.Add("@FlashcardIdParameter", flashcardId, DbType.Int32);
        }
        return _dapper.LoadDataWithParameters<Flashcard>(sql, sqlParameters);
    }

    // Insert or update flashcard (update if flashcardId does not exist)
    [HttpPut]
    public IActionResult UpsertDeck(Flashcard flashcardToAdd)
    {
        // Create flashcard if FlashcardId not provided, update otherwise
        string sql = @"EXEC VideoFlashcardsSchema.spFlashcards_Upsert
            @UserId = @UserIdParameter, @DeckId = @DeckIdParameter, @FlashcardUrl = @FlashcardUrlParameter, 
            @FlashcardTitle = @FlashcardTitleParameter, @FlashcardDescription = @FlashcardDescriptionParameter";
        DynamicParameters sqlParameters = new DynamicParameters();
        // Extract UserId from JWT
        sqlParameters.Add("@UserIdParameter", User.FindFirst("userId")?.Value, DbType.Int32);
        sqlParameters.Add("@DeckIdParameter", flashcardToAdd.DeckId, DbType.Int32);
        sqlParameters.Add("@FlashcardUrlParameter", flashcardToAdd.FlashcardUrl, DbType.String);
        sqlParameters.Add("@FlashcardTitleParameter", flashcardToAdd.FlashcardTitle, DbType.String);
        sqlParameters.Add("@FlashcardDescriptionParameter", flashcardToAdd.FlashcardDescription, DbType.String);
        // If FlashcardId exists
        if(flashcardToAdd.FlashcardId > 0)
        {
            sql += ", @FlashcardId = @FlashcardIdParameter";
            sqlParameters.Add("FlashcardIdParameter", flashcardToAdd.FlashcardId);
        }
        if(_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        }
        return StatusCode(400, "Flashcard could not be created or updated.");
    }

    // Delete flashcard given flashcardId
    [HttpDelete("{flashcardId}")]
    public IActionResult DeleteUser(int flashcardId)
    {
        // Executes spFlashcards_Delete specified in CreateDatabase.sql
        string sql = @"EXEC VideoFlashcardsSchema.spFlashcards_Delete
            @FlashcardId = @FlashcardIdParameter, @UserId = @UserIdParameter";
            
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@FlashcardIdParameter", flashcardId, DbType.Int32);
        // Obtain userId from JWT; make sure user own Flashcard
        sqlParameters.Add("@UserIdParameter", User.FindFirst("userId")?.Value, DbType.Int32);
        if(_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        }
        return StatusCode(400, "Failed to delete flashcard.");
    }
}