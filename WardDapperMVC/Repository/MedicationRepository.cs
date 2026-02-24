using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class MedicationRepository:IMedicationRepository
    {
        private readonly ISqlDataAccess _db;

        public MedicationRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddMedicationAsync(Medication medication)
        {
            try
            {
                await _db.SaveData("sp_Insert_Medication", new
                {
                    medication.MedicationType,
                    medication.MedicationName,
                    medication.Description
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

        public async Task<bool> DeleteMedicatonAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Medication", new { MedID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync()
        {
            string query = "sp_GetAll_Medication";
            return await _db.GetData<Medication, dynamic>(query, new { });
        }

        public async Task<Medication> GetMedicationByIdAsync(int id)
        {
            IEnumerable<Medication> result = await _db.GetData<Medication, dynamic>("sp_Get_Medication", new { MedID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateMedicationAsync(Medication medication)
        {
            try
            {
                await _db.SaveData("sp_update_Medication", medication);
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
