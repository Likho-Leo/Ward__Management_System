using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;
using System.Data;
using Dapper;

namespace WardDapperMVC.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnection _connection;

        public PatientRepository(ISqlDataAccess db, IDbConnection connection)
        {
            _db = db;
            _connection = connection;
        }

        //New
        public async Task<int> GetNewReferralCountAsync()
        {
            const string sql = "SELECT COUNT(*) FROM ReferPatient WHERE Status = @Status";

            var newReferralCount = await _connection.ExecuteScalarAsync<int>(sql, new { Status = "Refer" });  
            return newReferralCount;
        }

        public async Task<IEnumerable<PatientInstruction>> TrackVisit()
        {
            string query = "sp_track_visits";
            return await _db.GetData<PatientInstruction, dynamic>(query, new { });
        }

        public async Task<PatientReferrals> GetReferralByIdAsync(int id)
        {
                var query = "SELECT * FROM ReferPatient WHERE ReferPatientID = @Id";
                var referral = await _connection.QuerySingleOrDefaultAsync<PatientReferrals>(query, new { Id = id });
                return referral;  
        }

        public async Task<bool> UpdateReferralStatusAsync(int referralId, string status)
        {
            var query = "UPDATE ReferPatient SET Status = @Status WHERE ReferPatientID = @ReferralId";
            var parameters = new { Status = status, ReferralId = referralId };
            var result = await _connection.ExecuteAsync(query, parameters);
            return result > 0;
        }
        public async Task<Patient> GetLastPatientAsync()
        {
            const string query = "SELECT TOP 1 * FROM Patient ORDER BY PatientId DESC"; // Use TOP instead of LIMIT
            return await _connection.QuerySingleOrDefaultAsync<Patient>(query);
        }

        public async Task<bool> AddPatientAsync(Patient patient)
        {
            try
            {
                await _db.SaveData("sp_Insert_Patient", new
                {
                    patient.PatientNumber,
                    patient.FirstName,
                    patient.LastName,
                    patient.Title,
                    patient.Gender,
                    patient.DOB,
                    patient.IDNumber,
                    patient.Email,
                    patient.PhoneNumber,
                    patient.AddressLine1,
                    patient.AddressLine2,
                    patient.PostalCode,
                    patient.Province,
                    patient.Country,
                    patient.City,
                    patient.Town_Surburb,
                    patient.CurrentMedications,
                    patient.Allergies,
                    patient.MedicalHistory,
                    patient.NextOfKinName,
                    patient.NextOfKinPhoneNumber,
                    patient.NextOfKinRelationship,
                    patient.EmergencyContactName,
                    patient.EmergencyContactPhone,
                    patient.InsuranceProvider,
                    patient.PolicyNumber
                });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Patient", new { PatientId = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<PatientReferrals>> GetAllNewRefersAsync()
        {
            string query = "sp_GetAll_NewRefers";
            return await _db.GetData<PatientReferrals, dynamic>(query, new { });
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            string query = "sp_GetAll_Patients";
            return await _db.GetData<Patient, dynamic>(query, new { });
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            IEnumerable<Patient> result = await _db.GetData<Patient, dynamic>("sp_Get_Patient", new { PatientId = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            try
            {
                await _db.SaveData("sp_update_patient", new
                {
                    patient.PatientId,
                    patient.PatientNumber,
                    patient.FirstName,
                    patient.LastName,
                    patient.Title,
                    patient.Gender,
                    patient.DOB,
                    patient.IDNumber,
                    patient.Email,
                    patient.PhoneNumber,
                    patient.AddressLine1,
                    patient.AddressLine2,
                    patient.PostalCode,
                    patient.Province,
                    patient.Country,
                    patient.City,
                    patient.Town_Surburb,
                    patient.CurrentMedications,
                    patient.Allergies,
                    patient.MedicalHistory,
                    patient.NextOfKinName,
                    patient.NextOfKinPhoneNumber,
                    patient.NextOfKinRelationship,
                    patient.EmergencyContactName,
                    patient.EmergencyContactPhone,
                    patient.InsuranceProvider,
                    patient.PolicyNumber
                });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
