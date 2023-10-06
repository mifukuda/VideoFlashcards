using System.Data;
using Dapper;
using Server.Data;
using Server.Models;

namespace Server.Helpers
{
    // Helper methods for reused sql operations
    public class ReusableSql
    {
        private readonly DataContextDapper _dapper;

        public ReusableSql(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        // Insert or update user (used in UserController.js)
        public User UpsertUser (User user)
        {
            // Execute spUsers_Upsert stored procedure (defined in CreateDatabase.sql)
            string sql = @"EXEC VideoFlashcardsSchema.spUsers_Upsert
                @Email = @EmailParameter,
                @FirstName = @FirstNameParameter,
                @LastName = @LastNameParameter,
                @UserId = @UserIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
            sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
            sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
            sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

            return _dapper.LoadDataSingleWithParameters<User>(sql, sqlParameters);;
        }
    }
}