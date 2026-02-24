using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeletUserAsync(int id);

        Task<User> GetUserByIdAsync(int id);

        Task<IEnumerable<User>> GetAllUsersAsync();

        //New methods
        Task<User> GetUserByEmployeeNumberAsync(string employeeNumber);
        Task<string> GetLastEmployeeNumberAsync();

    }
}
