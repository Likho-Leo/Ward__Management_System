using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain.Nurse;



namespace WardDapperMVC.Repository.Nusrse
{
    public interface IDoctorAdviceRepo
    {
        Task<bool>AddDocAdviceAsync(DoctorAdvice doctorAdvice);
        Task<bool>UpdateDocAdviceAsync(DoctorAdvice doctorAdvice);
        Task<bool> DeleteDocAdviceAsync(int id);
        Task<DoctorAdvice>GetDocAdviceByIdAsync(int id);
        Task<IEnumerable<DoctorAdvice>> GetAllDocAdviceAsync();
        Task<IEnumerable<DoctorAdvice>> GetInstructionsAsync();
        Task<DoctorAdvice> GetDocInstructionByIdAsync(int id);
    }
}