using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Server.Data;
using Server.Helpers;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    // Wrapper for Dapper methods
    private readonly DataContextDapper _dapper;
    // Reused methods
    private readonly ReusableSql _reusableSql;

    public UsersController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        _reusableSql = new ReusableSql(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("Select GETDATE()");
    }

    // Get user(s)
    [HttpGet("{userId}")]
    public IEnumerable<User> GetUsers(int userId = 0)
    {
        // Stored procedure defined in CreateDatabase.sql
        string sql = "EXEC VideoFlashcardsSchema.spUsers_Get";
        DynamicParameters sqlParameters = new DynamicParameters();
        // If userId == 0, return all users; else, return one user
        if(userId != 0)
        {
            sql += " @UserId = @UserIdParameter";
            sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
        }
        IEnumerable<User> users = _dapper.LoadDataWithParameters<User>(sql, sqlParameters);
        return users;
    }

    // Insert or update user (update if UserId does not exist)
    [HttpPut]
    public IActionResult UpsertUser(User user)
    {
        // Executes spUsers_Upsert stored procedure
        if(_reusableSql.UpsertUser(user))
        {
            return Ok();
        }
        return StatusCode(400, "User could not be created or updated.");
    }

    // Delete user given userId
    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        // Executes spUsers_Delete specified in CreateDatabase.sql
        string sql = @"EXEC VideoFlashcardsSchema.spUsers_Delete
            @UserId = @UserIdParameter";
            
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
        if(_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        }
        return StatusCode(400, "Could not delete specified user");
    }
}
