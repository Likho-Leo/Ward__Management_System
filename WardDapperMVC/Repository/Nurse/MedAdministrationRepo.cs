using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Models.Domain.Nurse;

namespace WardDapperMVC.Repository.Nusrse
{
    public class MedAdministrationRepo : IMedAdministrationRepo
    {
        private readonly ISqlDataAccess _db;

        public MedAdministrationRepo(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddMedAdminAsync(MedAdministration medAdministration)
        {
            try
            {
                await _db.SaveData("sp_AddMedAdmin", new
                {
                    medAdministration.UserId,
                    medAdministration.PatientId,
                    medAdministration.Med,
                    medAdministration.Date,
                    medAdministration.status
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public async Task<bool> UpdateMedAdminAsync(MedAdministration medAdministration)
        //{
        //    try
        //    {
        //        await _db.SaveData("sp_UpdateMedAdmin", medAdministration);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
       public async Task<bool> UpdateMedAdminAsync(MedAdministration medAdministration)
        {
            if (medAdministration.AdminMedID == 0)
            {
                // Log the issue and return false
                Console.WriteLine("Error: AdminMedId is 0. Cannot update record.");
                return false;
            }

            try
            {
                // Map the patientVital properties to the parameters of the stored procedure
                var parameters = new
                {
                    Date = medAdministration.Date,
                    Status = medAdministration.status,
                    Med = medAdministration.Med,
                    AdminMedID = medAdministration.AdminMedID
                };

                await _db.SaveData("sp_UpdateMedAdmin", parameters);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Update failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteMedAdminAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_DeleteMedAdmin", new { ID = id });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<MedAdministration> GetMedAdminById(int id)
        {
            IEnumerable<MedAdministration> result = await _db.GetData<MedAdministration, dynamic>("sp_GetMedAdmin", new { ID = id });
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<MedAdministration>> GetAllMedAdminAsync()
        {
            return await _db.GetData<MedAdministration, dynamic>("sp_GetMedAdminRecords", new { });
        }

        // Implement the GetAllMedicationsAsync method 
        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync()
        {
            return await _db.GetData<Medication, dynamic>("sp_GetAllMedications", new { });
        }
    }
}
