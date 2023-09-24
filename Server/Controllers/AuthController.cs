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
}
