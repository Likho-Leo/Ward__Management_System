using Dapper;
using System.Data;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class ManagerRepository:IManagerRepository
    {
        private readonly IDbConnection _connection; //We use this if we dont want to do query as the stored procedure

        public ManagerRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        /* This method is for geting Manager details using the UserID which is the ForeignKey
          so that we can work with the the ManagerID in the ConsumableController*/
        public async Task<Manager> GetManagerByUserIdAsync(int userId)
        {
            var query = "SELECT * FROM Manager WHERE UserID = @UserId";
            return await _connection.QuerySingleOrDefaultAsync<Manager>(query, new { UserId = userId });
        }
    }
}
