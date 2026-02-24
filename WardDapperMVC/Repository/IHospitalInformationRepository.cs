using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IHospitalInformationRepository
    {
        Task<bool> AddHospitalInfoAsync(HospitalInformation hospitalInformation);
        Task<bool> UpdateHospitalInfoAsync(HospitalInformation hospitalInformation);
        Task<bool> DeleteHospitalInfoAsync(int id);

        Task<HospitalInformation> GetHospitalInfoByIdAsync(int id);

        Task<IEnumerable<HospitalInformation>> GetAllHospitalInfoAsync();
    }
}
