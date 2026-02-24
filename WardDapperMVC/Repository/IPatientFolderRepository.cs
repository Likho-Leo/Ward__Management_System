using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IPatientFolderRepository
    {
        Task<bool> AddPatientFolderAsync(PatientFolder patientFolder);
        Task<bool> UpdatePatientFolderAsync(PatientFolder patientFolder);
        Task<bool> DeletePatientFolderAsync(int id);

        Task<PatientFolder> GetPatientFolderByIdAsync(int id);

        Task<IEnumerable<PatientFolder>> GetAllPatientFoldersAsync();

        //new
        Task<PatientFolder> GetPatientByNumberAsync(string patientNumber);
    }
}
