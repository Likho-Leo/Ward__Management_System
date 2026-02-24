using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IConsumableRepository
    {
        Task<bool> AddConsumableAsync(Consumable consumable);
        Task<bool> UpdateConsumableAsync(Consumable consumable);
        Task<bool> DeleteConsumableAsync(int id);

        Task<Consumable> GetConsumableByIdAsync(int id);

        Task<IEnumerable<Consumable>> GetAllConsumablesAsync();
    }
}
