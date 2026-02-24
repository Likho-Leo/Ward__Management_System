using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;


namespace WardDapperMVC.Repository.Nusrse
{
    public interface IPatientRepo
    {
        Task<Patient?> GetPatientDetailsByNoAsync(string no);
    }
}