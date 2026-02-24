using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IBedRepository
    {
        Task<bool> AddBedAsync(Bed bed);
        Task<bool> UpdateBedAsync(Bed bed);
        Task<bool> DeleteBedAsync(int id);

        Task<Bed> GetBedByIdAsync(int id);

        Task<IEnumerable<Bed>> GetAllBedsAsync();

        //new methods
        Task UpdateBedAvailabilityAsync(int bedId, string status);
        Task<IEnumerable<Bed>> GetAllBedByAvailabiliyiStatus();
        Task<IEnumerable<Bed>> GetBedAvailabilityStatusForEditFolder();
        Task<IEnumerable<Bed>> GetBedsByWardAsync(int wardId);
        Task<Bed> GetBedsByIdAsync(int? bedId);
    }
}
