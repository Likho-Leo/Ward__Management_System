using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public class HospitalInformationRepository:IHospitalInformationRepository
    {
        private readonly ISqlDataAccess _db;

        public HospitalInformationRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddHospitalInfoAsync(HospitalInformation hospitalInformation)
        {
            try
            {
                await _db.SaveData("sp_Insert_HospitalInfo", new
                {
                    hospitalInformation.HospitalName,
                    Logo = hospitalInformation.Logo,
                    hospitalInformation.Slogan,
                    hospitalInformation.TellNO,
                    hospitalInformation.Email,
                    hospitalInformation.Address
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

        public async Task<bool> DeleteHospitalInfoAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_HospitalInfo", new { InfoID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<HospitalInformation>> GetAllHospitalInfoAsync()
        {
            string query = "sp_GetAll_HospitalInfo";
            return await _db.GetData<HospitalInformation, dynamic>(query, new { });
        }

        public async Task<HospitalInformation> GetHospitalInfoByIdAsync(int id)
        {
            IEnumerable<HospitalInformation> result = await _db.GetData<HospitalInformation, dynamic>("sp_Get_HospitalInfo", new { InfoID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateHospitalInfoAsync(HospitalInformation hospitalInformation)
        {
            try
            {
                await _db.SaveData("sp_update_HospitalInfo", new
                {
                    hospitalInformation.InfoID,
                    hospitalInformation.HospitalName,
                    Logo = hospitalInformation.Logo,
                    hospitalInformation.Slogan,
                    hospitalInformation.TellNO,
                    hospitalInformation.Email,
                    hospitalInformation.Address
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
