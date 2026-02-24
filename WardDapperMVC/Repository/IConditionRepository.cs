using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IConditionRepository
    {
        Task<bool> AddConditionAsync(Condition condition);
        Task<bool> UpdateConditionAsync(Condition condition);
        Task<bool> DeleteCondtionAsync(int id);

        Task<Condition> GetCondtionByIdAsync(int id);

        Task<IEnumerable<Condition>> GetAllConditionsAsync();
    }
}
