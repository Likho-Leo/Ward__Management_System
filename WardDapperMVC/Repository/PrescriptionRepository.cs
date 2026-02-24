using Dapper;
using System.Data;
using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnection _dbConnection;
        public PrescriptionRepository(ISqlDataAccess db, IDbConnection dbConnection)
        {
            _db = db;
            _dbConnection = dbConnection;
        }


        //New
        public async Task<Prescription> GetPatientByNumberAsync(string patientNumber)
        {
            var query = "SELECT * FROM Patient WHERE PatientNumber = @PatientNumber AND InActive = 'N'";
            return await _dbConnection.QueryFirstOrDefaultAsync<Prescription>(query, new { PatientNumber = patientNumber });
        }

        //New
        public async Task<Prescription> GetEmplyeeByNumberAsync(string employeeNumber)
        {
            var query = "SELECT * FROM Patient WHERE PatientNumber = @PatientNumber AND InActive = 'N'";
            return await _dbConnection.QueryFirstOrDefaultAsync<Prescription>(query, new { PatientNumber = employeeNumber });
        }

        public async Task<Prescription> GetPrescriptionDetailsAsync(int id)
        {
            IEnumerable<Prescription> result = await _db.GetData<Prescription, dynamic>("GetPrescriptionDetails", new { PrescriptionID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> AddAsync(Prescription script)
        {
            try
            {
                await _db.SaveData("sp_Insert_Prescription", new { script.Script, script.Date, script.ProcessStatus, script.PatientID,script.UserID});
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Prescription", new { PrescriptionID = id });
                return true;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            string query = "sp_GetAll_Prescriptions";
            return await _db.GetData<Prescription, dynamic>(query, new { });
        }
        public async Task<Prescription> GetByIdAsync(int id)
        {
            IEnumerable<Prescription> result = await _db.GetData<Prescription, dynamic>("sp_Get_Prescription", new { PrescriptionID = id });
            return result.FirstOrDefault();
        }
        public async Task<bool> UpdateAsync(Prescription script)
        {
            try
            {
                await _db.SaveData("sp_Update_Prescription", new { script.PrescriptionID,script.Script, script.Date, script.ProcessStatus });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}