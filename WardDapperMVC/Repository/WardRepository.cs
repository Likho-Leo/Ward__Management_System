using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class WardRepository:IWardRepository
    {
        private readonly ISqlDataAccess _db;

        public WardRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddWardAsync(Ward ward)
        {
            try
            {
                await _db.SaveData("sp_Insert_Ward", new
                {
                    ward.WardType,
                    ward.WardName,
                    ward.TotalBeds,
                    ward.Status
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

        public async Task<bool> DeleteWardAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Ward", new { WardID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Ward>> GetAllWardsAsync()
        {
            string query = "sp_GetAll_Ward";
            return await _db.GetData<Ward, dynamic>(query, new { });
        }

        public async Task<Ward> GetWardByIdAsync(int id)
        {
            IEnumerable<Ward> result = await _db.GetData<Ward, dynamic>("sp_Get_Ward", new { WardID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateWardAsync(Ward ward)
        {
            try
            {
                await _db.SaveData("sp_update_Ward", ward);
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
