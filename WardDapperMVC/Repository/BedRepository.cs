using Dapper;
using System.Data;
using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class BedRepository:IBedRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnection _dbConnection; //We use this if we dont want to do query as the stored procedure

        public BedRepository(ISqlDataAccess db, IDbConnection dbConnection)
        {
            _db = db;
            _dbConnection = dbConnection;
        }

        //New Methods that are also in the Interface

        /* This method is for me AND I am still testing it*/
        public async Task UpdateBedAvailabilityAsync(int bedId, string status)
        {
            var query = "UPDATE Bed SET BedAvailabilityStatus = @Status WHERE BedID = @BedID";
            var parameters = new { BedID = bedId, Status = status };
            await _dbConnection.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<Bed>> GetAllBedByAvailabiliyiStatus()
        {
            string query = "GetAllBedByAvailabiliyiStatus";
            return await _db.GetData<Bed, dynamic>(query, new { });
        }

        public async Task<IEnumerable<Bed>> GetBedAvailabilityStatusForEditFolder()
        {
            string query = "sp_GetBedAvailabilityStatusForEditFolder";
            return await _db.GetData<Bed, dynamic>(query, new { });
        }

        public async Task<IEnumerable<Bed>> GetBedsByWardAsync(int wardId)
        {
            var sql = "SELECT * FROM Bed WHERE WardId = @WardId AND BedAvailabilityStatus = 'Available' AND InActive='N'";
            return await _dbConnection.QueryAsync<Bed>(sql, new { WardID = wardId });
        }

        public async Task<Bed> GetBedsByIdAsync(int? bedId)
        {
            if (!bedId.HasValue)
                return null;

            const string sql = @"
            SELECT BedID, BedNo, BedAvailabilityStatus
            FROM Bed
            WHERE BedID = @BedID;";

            var parameters = new { BedID = bedId.Value };

            return await _dbConnection.QuerySingleOrDefaultAsync<Bed>(sql, parameters);
        }


        //And these are the one we all know
        public async Task<bool> AddBedAsync(Bed bed)
        {
            try
            {
                await _db.SaveData("sp_Insert_Bed", new
                {
                    bed.BedNo,
                    bed.BedAvailabilityStatus,
                    bed.WardId
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

        public async Task<bool> DeleteBedAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Bed", new { BedID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Bed>> GetAllBedsAsync()
        {
            string query = "sp_GetAll_Bed";
            return await _db.GetData<Bed, dynamic>(query, new { });

        }

        public async Task<Bed> GetBedByIdAsync(int id)
        {
            IEnumerable<Bed> result = await _db.GetData<Bed, dynamic>("sp_Get_Bed", new { BedID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateBedAsync(Bed bed)
        {
            try
            {
                await _db.SaveData("sp_update_Bed", new
                {
                    bed.BedID,
                    bed.BedNo,
                    bed.BedAvailabilityStatus,
                    bed.WardId
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
