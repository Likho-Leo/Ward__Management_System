using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;



namespace WardDapperMVC.Repository.Nusrse
{
    public interface IUserRepo
    {
        Task<User?> GetEmployeeDetailsByNoAsync(string no);
    }
}