using Dapper;
using System.Data;
using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class PatientMovementRepository:IPatientMovementRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnection _dbConnection;

        public PatientMovementRepository(ISqlDataAccess db, IDbConnection dbConnection)
        {
            _db = db;
            _dbConnection = dbConnection;
        }

        //New
        public async Task<PatientMovement> GetPatientByNumberAsync(string patientNumber)
        {
            var query = "SELECT * FROM Patient WHERE PatientNumber = @PatientNumber AND InActive = 'N'";
            return await _dbConnection.QueryFirstOrDefaultAsync<PatientMovement>(query, new { PatientNumber = patientNumber });
        }


        public async Task<bool> AddPatientMovementAsync(PatientMovement patientMovement)
        {
            try
            {
                await _db.SaveData("sp_Insert_Movement", new
                {
                    patientMovement.Status,
                    patientMovement.Date,
                    patientMovement.PatientId,
                    patientMovement.WardId,
                    patientMovement.BedID
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

        public async Task<bool> DeletePatientMovementAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Movement", new { MovementID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<PatientMovement>> GetAllPatientMovementsAsync()
        {
            string query = "sp_GetAll_Movement";
            return await _db.GetData<PatientMovement, dynamic>(query, new { });
        }

        public async Task<PatientMovement> GetPatientMovementByIdAsync(int id)
        {
            IEnumerable<PatientMovement> result = await _db.GetData<PatientMovement, dynamic>("sp_Get_Movement", new { MovementID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdatePatientMovementAsync(PatientMovement patientMovement)
        {
            try
            {
                await _db.SaveData("sp_update_Movement", new
                {
                    patientMovement.MovementID,
                    patientMovement.Status,
                    patientMovement.Date,
                    patientMovement.PatientId,
                    patientMovement.WardId,
                    patientMovement.BedID
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
