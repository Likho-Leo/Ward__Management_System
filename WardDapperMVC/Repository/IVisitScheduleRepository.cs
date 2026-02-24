using WardDapperMVC.Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardDapperMVC.Repository
{
    public interface IVisitScheduleRepository
    {
        Task<bool> AddAsync(VisitSchedule visit);
        Task<bool> UpdateAsync(VisitSchedule visit);
        Task<bool> DeleteAsync(int id);

        Task<VisitSchedule> GetByIdAsync(int id);

        Task<IEnumerable<VisitSchedule>> GetAllAsync();

        // New method for getting doctor full name
        Task<string> GetDoctorFullNameAsync(int doctorId);

        //Doctor login seperator
        Task<IEnumerable<VisitSchedule>> GetAllForDoctorAsync(int doctorId);

    }
}
