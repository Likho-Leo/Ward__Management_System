using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;
using Dapper;
using System.Data;
using System.Data.Common;

namespace WardDapperMVC.Repository
{

    public class ReferralsRepository : IPatientReferralsRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnection _dbConnection;
        public ReferralsRepository(ISqlDataAccess db, IDbConnection dbConnection)
        {
            _db = db;
            _dbConnection = dbConnection;
        }

        //New
        public async Task<PatientReferrals> GetEmployeeByNumberAsync(string employeeNumber)
        {
            var query = "SELECT * FROM Users WHERE EmployeeNumber = @EmployeeNumber AND InActive = 'N'";
            return await _dbConnection.QueryFirstOrDefaultAsync<PatientReferrals>(query, new { EmployeeNumber = employeeNumber });
        }

        public async Task<PatientReferrals> GetReferralDetailsAsync(int id)
        {
            var parameters = new { ReferralId = id }; // Create an anonymous object for parameters

            // Use the correct type arguments for GetData
            var referrals = await _db.GetData<PatientReferrals, dynamic>(
                "GetReferralDetails", parameters);

            return referrals.FirstOrDefault(); // Return the first or default result
        }

        public async Task<bool> AddAsync(PatientReferrals model)
        {
            try
            {
                await _db.SaveData("sp_Insert_Referral", new
                {
                    model.Status,
                    model.Date,
                    model.PatientLastName,
                    model.PatientFirstName,
                    model.UserID,
                    model.FirstName,
                    model.LastName
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

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Referral", new { ID = id });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<PatientReferrals>> GetAllAsync()
        {
            string query = "sp_GetAll_Referrals";
            return await _db.GetData<PatientReferrals, dynamic>(query, new { });
        }


        public async Task<PatientReferrals> GetByIdAsync(int id)
        {
            var parameters = new { ReferPatientID = id };
            var result = await _db.GetData<PatientReferrals, dynamic>("sp_Get_Referral", parameters);
            return result.FirstOrDefault();
        }


        public async Task<bool> UpdateAsync(PatientReferrals patients)
        {
            try
            {

                await _db.SaveData("sp_update_Referral", patients);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //public async Task<string> GetDoctorNameByIdAsync(int doctorId)
        //{
        //    var doctor = await _db.GetData<User, dynamic>("sp_Get_Doctor_By_Id", new { DoctorID = doctorId });
        //    return doctor.FirstOrDefault()?.LastName; // This is the surname of the doctor
        //}

    }
}