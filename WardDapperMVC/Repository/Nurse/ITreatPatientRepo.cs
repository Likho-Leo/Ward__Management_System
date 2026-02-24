using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;



namespace WardDapperMVC.Repository.Nusrse
{
    public interface ITreatPatientRepo
    {
        Task<bool> AddTreatmentAsync(TreatPatient treatPatient);
        Task<bool> UpdateTreatmentAsync(TreatPatient treatPatient);
        Task<bool> DeleteTreatmentAsync(int id);
        Task<TreatPatient> GetTreatmentByIdAsync(int id);
        Task<IEnumerable<TreatPatient>> GetAllTreatmentsAsync();
    }
}