using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;


namespace WardDapperMVC.Repository.Nusrse
{
    public interface IPatientVitalRepo
    {
        Task<bool> CreateRecordAsync(PatientVital patientVital);
        Task<bool> UpdateRecordAsync(PatientVital patientVital);
        Task<bool> DeleteRecordAsync(int id);
        Task<PatientVital> GetRecordByIDAsync(int id);
        //Task<PatientVital> GetPatientDetailsByNoAsync(int patientNo);
        Task<IEnumerable<PatientVital?>> GetRecordsAsync();
    }
}