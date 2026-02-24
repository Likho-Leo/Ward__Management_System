using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IManagerRepository
    {
        Task<Manager> GetManagerByUserIdAsync(int userId);
    }
}
