using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IPatientMovementRepository
    {
        Task<bool> AddPatientMovementAsync(PatientMovement patientMovement);
        Task<bool> UpdatePatientMovementAsync(PatientMovement patientMovement);
        Task<bool> DeletePatientMovementAsync(int id);

        Task<PatientMovement> GetPatientMovementByIdAsync(int id);

        Task<IEnumerable<PatientMovement>> GetAllPatientMovementsAsync();

        //new
        Task<PatientMovement> GetPatientByNumberAsync(string patientNumber);
    }
}
