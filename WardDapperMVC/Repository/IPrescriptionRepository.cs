using WardDapperMVC.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WardDapperMVC.Repository
{
    public interface IPrescriptionRepository
    {
        Task<bool> AddAsync(Prescription script);
        Task<bool> UpdateAsync(Prescription script);
        Task<bool> DeleteAsync(int id);
        Task<Prescription> GetByIdAsync(int id);
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription> GetPrescriptionDetailsAsync(int id);
        //new
        Task<Prescription> GetPatientByNumberAsync(string patientNumber);
        //new
        Task<Prescription> GetEmplyeeByNumberAsync(string employeeNumber);
    }
}