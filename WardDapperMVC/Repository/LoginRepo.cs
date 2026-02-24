using Dapper;
using System.Data;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class LoginRepo:ILoginRepo
    {
        private readonly IDbConnection _connection; //We use this if we dont want to do query as the stored procedure

        public LoginRepo(IDbConnection connection)
        {    
            _connection = connection;
        }

        // Fetch user by email
        public async Task<User?> Login(string email, string password)
        {
            try
            {
                var query = "SELECT * FROM Users WHERE Email = @Email AND InActive = 'N'";
                var user = await _connection.QueryFirstOrDefaultAsync<User?>(query, new { Email = email });

                // If user not found or password is incorrect
                if (user == null || user.Password != password) // Change this to a secure hash check when applicable
                {
                    // Return null for invalid email/password combination
                    return null;
                }

                return user; // Return user if valid
            }
            catch (Exception ex)
            {
                //Log the exception if needed
                throw new Exception("An error occurred while logging in.", ex);

            }
        }

        // Fetch user role by email
        public async Task<string> GetUserRoleByEmailAsync(string email)
        {
            var query = "SELECT Role FROM Users WHERE Email = @Email AND InActive = 'N'";
            return await _connection.QuerySingleOrDefaultAsync<string>(query, new { Email = email });
        }
        // Fetch user neme by email
        public async Task<string> GetUserNameByEmailAsync(string email)
        {
            var query = "SELECT FirstName FROM Users WHERE Email = @Email AND InActive = 'N'";
            return await _connection.QuerySingleOrDefaultAsync<string>(query, new { Email = email });
        }
    }
}
