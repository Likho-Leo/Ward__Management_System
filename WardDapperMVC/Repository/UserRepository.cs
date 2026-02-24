using Dapper;
using System.Data;
using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnection _connection; //We use this if we dont want to do query as the stored procedure

        public UserRepository(ISqlDataAccess db, IDbConnection connection)
        {
            _db = db;
            _connection = connection;
        }

        //New Methods

        /*This method is used to get user details based on the EmployeeNumber that was entered.
         We use it in the ConsumableController*/
        public async Task<User> GetUserByEmployeeNumberAsync(string employeeNumber)
        {
            var query = "SELECT * FROM Users WHERE EmployeeNumber = @EmployeeNumber AND Role='StockManager' AND InActive = 'N'";
            return await _connection.QuerySingleOrDefaultAsync<User>(query, new { EmployeeNumber = employeeNumber });
        }

        public async Task<string> GetLastEmployeeNumberAsync()
        {
            var query = "SELECT TOP 1 EmployeeNumber FROM Users ORDER BY UserID DESC"; // Adjust for your SQL dialect
            var lastEmployeeNumber = await _connection.QuerySingleOrDefaultAsync<string>(query);
            return lastEmployeeNumber;
        }

        //And these are the one we all know
        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _db.SaveData("sp_Insert_User", new
                {
                    user.EmployeeNumber,
                    user.FirstName,
                    user.LastName,
                    user.Title,
                    user.Gender,
                    user.IDNo,
                    user.DOB,
                    user.PhoneNumber,
                    user.Email,
                    user.Password,
                    user.AddressLine1,
                    user.AddressLine2,
                    user.PostalCode,
                    user.Province,
                    user.Country,
                    user.City,
                    user.Town_Surburb,
                    user.Role,
                    user.EmploymentDate
                });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeletUserAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_User", new { UserID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            string query = "sp_GetAll_User";
            return await _db.GetData<User, dynamic>(query, new { });
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            IEnumerable<User> result = await _db.GetData<User, dynamic>("sp_Get_User", new { UserID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                await _db.SaveData("sp_update_User", user);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
