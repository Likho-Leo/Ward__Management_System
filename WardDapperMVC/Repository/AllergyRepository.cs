using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class AllergyRepository:IAllergyRepository
    {
        private readonly ISqlDataAccess _db;

        public AllergyRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddAllergyAsync(Allergy allergy)
        {
            try
            {
                await _db.SaveData("sp_Insert_Allergy", new
                {
                    allergy.Allergen, 
                    allergy.AllergyType,
                    allergy.Symptoms
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

        public async Task<bool> DeleteAllergyAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Allergy", new { AllergyID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Allergy>> GetAllAllergiesAsync()
        {
            string query = "sp_GetAll_Allergy";
            return await _db.GetData<Allergy, dynamic>(query, new { });
        }

        public async Task<Allergy> GetAllergyByIdAsync(int id)
        {
            IEnumerable<Allergy> result = await _db.GetData<Allergy, dynamic>("sp_Get_Allergy", new { AllergyID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateAllergyAsync(Allergy allergy)
        {
            try
            {
                await _db.SaveData("sp_update_Allergy", allergy);
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
