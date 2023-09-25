using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Server.Data;
using Server.Dtos;
using Server.Helpers;
using Server.Models;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    // Wrapper for Dapper methods
    private readonly DataContextDapper _dapper;
    // Reused methods
    private readonly ReusableSql _reusableSql;
    private readonly AuthHelper _authHelper;

    public AuthController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        _reusableSql = new ReusableSql(config);
        _authHelper = new AuthHelper(config);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        // Check is given passwords match
        if(userForRegistration.Password == userForRegistration.PasswordConfirm)
        {
            // Query Auth table to see if account with email already exists
            string sqlCheckUserExists = "Select Email FROM VideoFlashcardsSchema.Auth WHERE Email = '"
                + userForRegistration.Email + "'";
            IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
            // No users with specified email
            if(existingUsers.Count() == 0)
            {
                // Format into DTO with fields Email and Password
                UserForLoginDto userForSetPassword = new UserForLoginDto()
                {
                    Email = userForRegistration.Email,
                    Password = userForRegistration.Password
                };
                // Add Email, PasswordHash, and PasswordSalt to Auth table
                if(_authHelper.SetPassword(userForSetPassword))
                {
                    User user = new User()
                    {
                        Email = userForRegistration.Email,
                        FirstName = userForRegistration.FirstName,
                        LastName = userForRegistration.LastName
                    };
                    // Create row in user table
                    if(_reusableSql.UpsertUser(user))
                    {
                        return Ok();
                    }
                    return StatusCode(400, "Failed to create user");
                }
                return StatusCode(400, "Failed to register user");
            }
            return StatusCode(400, "Account with this email already exists");
        }
        return StatusCode(400, "Passwords do not match");
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        // Execute spLoginConfirmation_Get stored procedure defined in CreateDatabase.SQL
        string sqlForHashAndSalt = @"EXEC VideoFlashcardsSchema.spLoginConfirmation_Get
            @Email = @EmailParam";
        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@EmailParam", userForLogin.Email, DbType.String);

        // Get PasswordHash and PasswordSalt
        UserForLoginConfirmationDto userForConfirmation = _dapper.LoadDataSingleWithParameters<UserForLoginConfirmationDto>(sqlForHashAndSalt, sqlParameters);

        // Iterate through new password hash and password hash retrieved from database
        byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);
        for(int index = 0; index < passwordHash.Length; index++)
        {
            if (passwordHash[index] != userForConfirmation.PasswordHash[index])
            {
                return StatusCode(401, "Incorrect password!");
            }
        }

        String sqlUserId = "SELECT UserId FROM VideoFlashcardsSchema.Users WHERE Email = '" + userForLogin.Email + "'";
        int userId = _dapper.LoadDataSingle<int>(sqlUserId);
        return Ok(new Dictionary<string, string>{
            {"token", _authHelper.CreateToken(userId)}
        });
    }

    [HttpGet("RefreshToken")]
    public string RefreshToken()
    {
        // Make sure UserId exists
        string sqlUserId = "Select UserId FROM VideoFlashcardsSchema.Users WHERE UserId = '" + User.FindFirst("userId")?.Value + "'";
        int userId = _dapper.LoadDataSingle<int>(sqlUserId);
        return _authHelper.CreateToken(userId);
    }
}
