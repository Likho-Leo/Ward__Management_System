using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;


namespace WardDapperMVC.Repository.Nusrse
{
    public class DoctorAdviceRepo:IDoctorAdviceRepo
    {
        private readonly ISqlDataAccess _db;

        public DoctorAdviceRepo(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool>AddDocAdviceAsync(DoctorAdvice doctorAdvice)
        {
            try
            {
                await _db.SaveData("sp_AddDocAdvice", new { 
                    doctorAdvice.PatientID,
                    doctorAdvice.DoctorNote, 
                    doctorAdvice.DischargeStatus,
                    doctorAdvice.DateOfVisit, 
                    doctorAdvice.DoctorID });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool>UpdateDocAdviceAsync(DoctorAdvice doctorAdvice)
        {
            try
            {
                await _db.SaveData("sp_UpdateDocAdvice", new { doctorAdvice });
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool>DeleteDocAdviceAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_DeleteDocAdvice", new { ID = id });
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public async Task<DoctorAdvice> GetDocAdviceByIdAsync(int id)
        {
            IEnumerable<DoctorAdvice> result = await _db.GetData<DoctorAdvice, dynamic>("sp_GetDocAdvice", new { InstructionID = id });
            if (result == null || !result.Any())
            {
                // Handle the case where no record is found
                Console.WriteLine("No record found for the provided InstructionID.");
                return null;
            }
            return result.FirstOrDefault();
        }

        public async Task<DoctorAdvice> GetDocInstructionByIdAsync(int id)
        {
            IEnumerable<DoctorAdvice> result = await _db.GetData<DoctorAdvice, dynamic>("sp_GetInstruction", new { InstructionID = id });
            if (result == null || !result.Any())
            {
                // Handle the case where no record is found
                Console.WriteLine("No record found for the provided InstructionID.");
                return null;
            }
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<DoctorAdvice>> GetAllDocAdviceAsync()
        {
            return await _db.GetData<DoctorAdvice, dynamic>("sp_GetAllDocAdvice", new { });
        }
        public async Task<IEnumerable<DoctorAdvice>> GetInstructionsAsync()
        {
            return await _db.GetData<DoctorAdvice, dynamic>("sp_GetInstructions", new { });
        }

       
    }
}
