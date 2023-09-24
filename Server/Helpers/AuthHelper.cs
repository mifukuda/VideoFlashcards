using System.Data;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Server.Data;
using Server.Dtos;

namespace Server.Helpers
{
    // Helper methods used in AuthController.cs
    public class AuthHelper
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;

        public AuthHelper(IConfiguration config)
        {
            _config = config;
            _dapper = new DataContextDapper(config);
        }

        // Salt and hash given password
        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            // Combine hash and salt
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );
        }

        // Creates/alters entry in Auth table; register user
        public bool SetPassword(UserForLoginDto userForSetPassword)
        {
            byte[] passwordSalt = new byte[128 / 8];
            // Generate random salt
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            // Salt and hash password
            byte[] passwordHash = GetPasswordHash(userForSetPassword.Password, passwordSalt);

            // Execute spRegistration_Upsert defined in CreateDatabase.sql
            string sqlAddAuth = @"EXEC VideoFlashcardsSchema.spRegistration_Upsert
                @Email = @EmailParameter, 
                @PasswordHash = @PasswordHashParameter,
                @PasswordSalt = @PasswordSaltParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@EmailParameter", userForSetPassword.Email, DbType.String);
            sqlParameters.Add("@PasswordHashParameter", passwordHash, DbType.Binary);
            sqlParameters.Add("@PasswordSaltParameter", passwordSalt, DbType.Binary);

            return _dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
        }
    }
}