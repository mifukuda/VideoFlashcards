using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Server.Data;
using Server.Helpers;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class DecksController : ControllerBase
{
    // Wrapper for Dapper methods
    private readonly DataContextDapper _dapper;

    public DecksController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    // Get userId from JWT token and return all user decks
    [HttpGet]
    public IEnumerable<Deck> GetDecks()
    {
        string sql = "EXEC VideoFlashcardsSchema.spDecks_Get @UserId = @UserIdParameter";
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("UserIdParameter", User.FindFirst("userId")?.Value, DbType.Int32);
        return _dapper.LoadDataWithParameters<Deck>(sql, sqlParameters);
    }

    // Insert or update deck (update if DeckId does not exist)
    [HttpPut]
    public IActionResult UpsertDeck(Deck deckToAdd)
    {
        // Update deck if DeckId not provided, update otherwise
        string sql = @"EXEC VideoFlashcardsSchema.spDecks_Upsert
            @UserId = @UserIdParameter, @DeckName = @DeckNameParameter";
        DynamicParameters sqlParameters = new DynamicParameters();
        // Extract UserId from JWT
        sqlParameters.Add("@UserIdParameter", User.FindFirst("userId")?.Value, DbType.Int32);
        sqlParameters.Add("@DeckNameParameter", deckToAdd.DeckName, DbType.String);
        // If DeckId exists
        if(deckToAdd.DeckId > 0)
        {
            sql += ", @DeckId = @DeckIdParameter";
            sqlParameters.Add("DeckIdParameter", deckToAdd.DeckId);
        }
        if(_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        }
        return StatusCode(400, "Deck could not be created or updated.");
    }

    // Delete user given userId
    [HttpDelete("{deckId}")]
    public IActionResult DeleteUser(int deckId)
    {
        // Executes spDecks_Delete specified in CreateDatabase.sql
        string sql = @"EXEC VideoFlashcardsSchema.spDecks_Delete
            @DeckId = @DeckIdParameter, @UserId = @UserIdParameter";
            
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@DeckIdParameter", deckId, DbType.Int32);
        sqlParameters.Add("@UserIdParameter", User.FindFirst("userId")?.Value, DbType.Int32);
        if(_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        }
        return StatusCode(400, "Failed to delete deck.");
    }
}