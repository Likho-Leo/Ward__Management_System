using Dapper;
using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;
using System.Data;

namespace WardDapperMVC.Repository
{
    public class PatientFolderRepository:IPatientFolderRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnection _dbConnection;

        public PatientFolderRepository(ISqlDataAccess db, IDbConnection dbConnection)
        {
            _db = db;
            _dbConnection = dbConnection;
        }

        //New
        public async Task<PatientFolder> GetPatientByNumberAsync(string patientNumber)
        {
            var query = "SELECT * FROM Patient WHERE PatientNumber = @PatientNumber AND InActive = 'N'";
            return await _dbConnection.QueryFirstOrDefaultAsync<PatientFolder>(query, new { PatientNumber = patientNumber });
        }

        public async Task<bool> AddPatientFolderAsync(PatientFolder patientFolder)
        {
            try
            {
                await _db.SaveData("sp_Insert_Folder", new
                {
                    patientFolder.Status,
                    patientFolder.AdmitDate,
                    patientFolder.PatientId,
                    patientFolder.WardId,
                    patientFolder.BedID
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

        public async Task<bool> DeletePatientFolderAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Folder", new { FolderID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<PatientFolder>> GetAllPatientFoldersAsync()
        {
            string query = "sp_GetAll_Folder";
            return await _db.GetData<PatientFolder, dynamic>(query, new { });
        }

        public async Task<PatientFolder> GetPatientFolderByIdAsync(int id)
        {
            IEnumerable<PatientFolder> result = await _db.GetData<PatientFolder, dynamic>("sp_Get_Folder", new { FolderID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdatePatientFolderAsync(PatientFolder patientFolder)
        {
            try
            {
                await _db.SaveData("sp_update_Folder", new
                {
                    patientFolder.FolderID,
                    patientFolder.Status,
                    patientFolder.AdmitDate,
                    patientFolder.PatientId,
                    patientFolder.WardId,
                    patientFolder.BedID
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
