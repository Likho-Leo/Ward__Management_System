using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class ConditionRepository : IConditionRepository
    {
        private readonly ISqlDataAccess _db;

        public ConditionRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddConditionAsync(Condition condition)
        {
            try
            {
                await _db.SaveData("sp_Insert_Condition", new
                {
                    condition.Conditions,
                    condition.ConditionType
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

        public async Task<bool> DeleteCondtionAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Condition", new { ConditionID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Condition>> GetAllConditionsAsync()
        {
            string query = "sp_GetAll_Condition";
            return await _db.GetData<Condition, dynamic>(query, new { });
        }

        public async Task<Condition> GetCondtionByIdAsync(int id)
        {
            IEnumerable<Condition> result = await _db.GetData<Condition, dynamic>("sp_Get_Condition", new { ConditionID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateConditionAsync(Condition condition)
        {
            try
            {
                await _db.SaveData("sp_update_Condition", condition);
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
