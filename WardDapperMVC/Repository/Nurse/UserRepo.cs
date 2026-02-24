using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;


namespace WardDapperMVC.Repository.Nusrse
{
    public class UserRepo:IUserRepo
    {
        private readonly ISqlDataAccess _db;

        public UserRepo(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<User?> GetEmployeeDetailsByNoAsync(string no)
        {
            IEnumerable<User> result = await _db.GetData<User, dynamic>("sp_GetEmployeeDetailsByNo", new { EmployeeNo = no });
            return result.FirstOrDefault();
        }
    }
}
