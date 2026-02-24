using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IWardRepository
    {
        Task<bool> AddWardAsync(Ward ward);
        Task<bool> UpdateWardAsync(Ward ward);
        Task<bool> DeleteWardAsync(int id);

        Task<Ward> GetWardByIdAsync(int id);

        Task<IEnumerable<Ward>> GetAllWardsAsync();
    }
}
