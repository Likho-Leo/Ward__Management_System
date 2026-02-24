using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;


namespace WardDapperMVC.Repository.Nusrse
{
    public class PatientRepo:IPatientRepo
    {
        private readonly ISqlDataAccess _db;

        public PatientRepo(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Patient?> GetPatientDetailsByNoAsync(string no)
        {
            IEnumerable<Patient> result = await _db.GetData<Patient, dynamic>("sp_GetPatientDetailsByNo", new { PatientNo = no });
            return result.FirstOrDefault();
        }
    } 
}
