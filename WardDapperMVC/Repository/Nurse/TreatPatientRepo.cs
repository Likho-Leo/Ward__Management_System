using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;

namespace WardDapperMVC.Repository.Nusrse

{
    public class TreatPatientRepo : ITreatPatientRepo
    {
        private readonly ISqlDataAccess _db;

        public TreatPatientRepo(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddTreatmentAsync(TreatPatient treatPatient)
        {
            try
            {
                await _db.SaveData("sp_AddTreatment", new { 
                    treatPatient.PatientId,
                    treatPatient.TreatmentType, 
                    treatPatient.Date,
                    treatPatient.UserId});
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //public async Task<bool>UpdateTreatmentAsync(TreatPatient treatPatient)
        //{
        //    try
        //    {
        //        await _db.SaveData("sp_UpdateTreatment", new { treatPatient });
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        public async Task<bool> UpdateTreatmentAsync(TreatPatient treatPatient)
        {
            if (treatPatient.TreatPatientID == 0)
            {
                // Log the issue and return false
                Console.WriteLine("Error: TreatPatientID is 0. Cannot update record.");
                return false;
            }

            try
            {
                // Map the patientVital properties to the parameters of the stored procedure
                var parameters = new
                {
                    TreatmentType = treatPatient.TreatmentType,
                    Date = treatPatient.Date,
                    TreatPatientID = treatPatient.TreatPatientID
                };

                await _db.SaveData("sp_UpdateTreatment", parameters);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Update failed: {ex.Message}");
                return false;
            }
        }
        public async Task<bool>DeleteTreatmentAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_DeleteTreatment", new { ID = id });
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<TreatPatient>GetTreatmentByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("InstructionID cannot be 0");
            }

            IEnumerable<TreatPatient> result = await _db.GetData<TreatPatient, dynamic>("sp_GetTreatmentById", new { TreatPatientID = id });
            return result.FirstOrDefault();
        }
        public async Task<IEnumerable<TreatPatient>> GetAllTreatmentsAsync()
        {
            return await _db.GetData<TreatPatient, dynamic>("sp_GetAllTreatments", new { });
        }
    }
    
}
