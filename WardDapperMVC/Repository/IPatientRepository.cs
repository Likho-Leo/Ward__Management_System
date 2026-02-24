using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IPatientRepository
    {
        Task<bool> AddPatientAsync(Patient patient);
        Task<bool> UpdatePatientAsync(Patient patient);
        Task<bool> DeletePatientAsync(int id);

        Task<Patient> GetPatientByIdAsync(int id);

        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<IEnumerable<PatientReferrals>> GetAllNewRefersAsync();
        Task<PatientReferrals> GetReferralByIdAsync(int id);
        // New method for updating referral status
        Task<bool> UpdateReferralStatusAsync(int referralId, string status);
        Task<Patient> GetLastPatientAsync(); // Add this line
        Task<int> GetNewReferralCountAsync();

        Task<IEnumerable<PatientInstruction>> TrackVisit();
    }
}
