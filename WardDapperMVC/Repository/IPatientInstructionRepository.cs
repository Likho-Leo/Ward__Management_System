using System.Collections.Generic;
using System.Threading.Tasks;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IPatientInstructionRepository
    {
        Task<bool> AddAsync(PatientInstruction instruction);
        Task<bool> UpdateAsync(PatientInstruction instruction);
        Task<bool> DeleteAsync(int id);
        Task<PatientInstruction> GetByIdAsync(int id);
        Task<IEnumerable<PatientInstruction>> GetAllAsync();
    }
}
