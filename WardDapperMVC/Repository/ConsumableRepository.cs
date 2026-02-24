using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class ConsumableRepository: IConsumableRepository
    {
        private readonly ISqlDataAccess _db;

        public ConsumableRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddConsumableAsync(Consumable consumable)
        {
            try
            {
                await _db.SaveData("sp_Insert_Consumables", new
                {
                    consumable.ConsumableType,
                    consumable.StockOnHand,
                    consumable.ParLevel,
                    consumable.ReorderPoint,
                    consumable.ManagerID
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

        public async Task<bool> DeleteConsumableAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Consumables", new { ConsumableID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Consumable>> GetAllConsumablesAsync()
        {
            string query = "sp_GetAll_Consumables";
            return await _db.GetData<Consumable, dynamic>(query, new { });
        }

        public async Task<Consumable> GetConsumableByIdAsync(int id)
        {
            IEnumerable<Consumable> result = await _db.GetData<Consumable, dynamic>("sp_Get_Consumables", new { ConsumableID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateConsumableAsync(Consumable consumable)
        {
            try
            {
                await _db.SaveData("sp_update_Consumables", new
                {
                    consumable.ConsumableID,
                    consumable.ConsumableType,
                    consumable.StockOnHand,
                    consumable.ParLevel,
                    consumable.ReorderPoint,
                    consumable.ManagerID
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
