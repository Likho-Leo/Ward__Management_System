using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IDischargePatientRepository
    {
        Task AddDischargeRecordAsync(DischargePatient discharge);
    }
}
