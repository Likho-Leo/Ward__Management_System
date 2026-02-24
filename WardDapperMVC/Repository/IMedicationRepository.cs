using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IMedicationRepository
    {
        Task<bool> AddMedicationAsync(Medication medication);
        Task<bool> UpdateMedicationAsync(Medication medication);
        Task<bool> DeleteMedicatonAsync(int id);

        Task<Medication> GetMedicationByIdAsync(int id);

        Task<IEnumerable<Medication>> GetAllMedicationsAsync();
    }
}
