using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Models.Domain.Nurse;



namespace WardDapperMVC.Repository.Nusrse
{
    public interface IMedAdministrationRepo
    {
        Task<bool> AddMedAdminAsync(MedAdministration medAdministration);
        Task<bool> UpdateMedAdminAsync(MedAdministration medAdministration);
        Task<bool> DeleteMedAdminAsync(int id);
        Task<MedAdministration> GetMedAdminById(int id);
        Task<IEnumerable<MedAdministration>> GetAllMedAdminAsync();
        Task<IEnumerable<Medication>> GetAllMedicationsAsync();
    }
}