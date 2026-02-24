using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IAllergyRepository
    {
        Task<bool> AddAllergyAsync(Allergy allergy);
        Task<bool> UpdateAllergyAsync(Allergy allergy);
        Task<bool> DeleteAllergyAsync(int id);

        Task<Allergy> GetAllergyByIdAsync(int id);

        Task<IEnumerable<Allergy>> GetAllAllergiesAsync();
    }
}
