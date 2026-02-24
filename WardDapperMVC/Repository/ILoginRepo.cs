using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface ILoginRepo
    {
        Task<User?> Login(string email, string password);
        Task<string> GetUserRoleByEmailAsync(string email);
        Task<string> GetUserNameByEmailAsync(string email);
    }
}