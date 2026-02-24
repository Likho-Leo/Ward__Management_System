using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;


namespace WardDapperMVC.Repository.Nusrse
{
    public class PatientVitalRepo:IPatientVitalRepo
    {
        private readonly ISqlDataAccess _db;

        public PatientVitalRepo(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool>CreateRecordAsync(PatientVital patientVital)
        {
            try
            {
                await _db.SaveData("sp_RecordVitals", new
                {
                    patientVital.UserId,
                    patientVital.PatientId,
                    patientVital.BloodPressure, 
                    patientVital.BloodOxygen,
                    patientVital.BloodGlucoseLvl, 
                    patientVital.PulseRate,
                    patientVital.RespirationRate,
                    patientVital.BodyTemp,
                    patientVital.Weight,
                    patientVital.Date
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool>DeleteRecordAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_DeleteVitals", new { VitalId = id });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<PatientVital>> GetRecordsAsync()
        {
            return await _db.GetData<PatientVital, dynamic>("sp_getPatientVital", new { });
        }
        public async Task<PatientVital?> GetRecordByIDAsync(int id)
        {
            // Debugging: Check the value of id before making the database call
            if (id == 0)
            {
                throw new ArgumentException("InstructionID cannot be 0");
            }

            IEnumerable<PatientVital> result = await _db.GetData<PatientVital, dynamic>("sp_GetPatientVitalsById", new { VitalId = id });
            return result.FirstOrDefault();
        }

        //public async Task<PatientVital?> GetPatientDetailsByNoAsync(int patientNo)
        //{
        //    // Debugging: Check the value of id before making the database call
        //    if (patientNo == 0)
        //    {
        //        throw new ArgumentException("Patient Number cannot be 0");
        //    }

        //    IEnumerable<PatientVital> details = await _db.GetData<PatientVital, dynamic>("sp_GetPatientDetailsByNo", new {PatientNumber =  patientNo});
        //    return details.FirstOrDefault();
        //}

        public async Task<bool> UpdateRecordAsync(PatientVital patientVital)
        {
            if (patientVital.VitalId == 0)
            {
                // Log the issue and return false
                Console.WriteLine("Error: VitalId is 0. Cannot update record.");
                return false;
            }

            try
            {
                // Map the patientVital properties to the parameters of the stored procedure
                var parameters = new
                {
                    BloodPressure = patientVital.BloodPressure,
                    BloodOxygen = patientVital.BloodOxygen,
                    BloodGlucoseLvl = patientVital.BloodGlucoseLvl,
                    PulseRate = patientVital.PulseRate,
                    RespirationRate = patientVital.RespirationRate,
                    BodyTemp = patientVital.BodyTemp,
                    Weight = patientVital.Weight,
                    Date = patientVital.Date,
                    VitalID = patientVital.VitalId
                };

                await _db.SaveData("sp_UpdateVitals", parameters);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Update failed: {ex.Message}");
                return false;
            }
        }


    }
}
