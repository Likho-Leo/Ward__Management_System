using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WardDapperMVC.Models.Domain;

namespace WardDapperMVC.Repository
{
    public interface IPatientReferralsRepository
    {
        Task<IEnumerable<PatientReferrals>> GetAllAsync(); // Retrieve all referrals
        Task<PatientReferrals> GetByIdAsync(int id); // Retrieve a specific referral by ID
        Task<bool> AddAsync(PatientReferrals model); // Create a new referral
        Task<bool> UpdateAsync(PatientReferrals referral); // Update an existing referral
        Task<bool> DeleteAsync(int id); // Delete a referral by ID

        Task<PatientReferrals> GetReferralDetailsAsync(int id); // Retrieve details of a specific referral

        //new
        Task<PatientReferrals> GetEmployeeByNumberAsync(string employeeNumber);
    }
}
