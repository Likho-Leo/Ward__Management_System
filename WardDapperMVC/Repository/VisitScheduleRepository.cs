using WardDapperMVC.DataAccess;
using WardDapperMVC.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardDapperMVC.Repository
{
    public class VisitScheduleRepository : IVisitScheduleRepository
    {
        private readonly ISqlDataAccess _db;

        public VisitScheduleRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(VisitSchedule visit)
        {
            try
            {
                var parameters = new
                {
                    visit.VisitType,
                    Date = visit.Date.ToString("yyyy-MM-dd"), // Format DateTime for SQL
                    visit.DoctorID,
                    visit.PatientID,
                    visit.InActive,
                    visit.FollowUpAppointmentDate
                };
                await _db.SaveData("sp_Create_Schedule", parameters);
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Instruction", new { ID = id });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<VisitSchedule>> GetAllAsync()
        {
            string query = "sp_Get_Schedules";
            return await _db.GetData<VisitSchedule, dynamic>(query, new { });
        }

        public async Task<VisitSchedule> GetByIdAsync(int id)
        {
            IEnumerable<VisitSchedule> result = await _db.GetData<VisitSchedule, dynamic>("sp_Get_Schedule", new { ScheduleID = id });
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdateAsync(VisitSchedule visit)
        {
            try
            {

                await _db.SaveData("sp_update_Visit", visit);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // New method implementation
        public async Task<string> GetDoctorFullNameAsync(int doctorId)
        {
            var result = await _db.GetData<VisitSchedule, dynamic>(
                "sp_Get_DoctorFullName",
                new { DoctorID = doctorId }
            );
            return result.FirstOrDefault()?.DoctorFullName;
        }

        //Login Seperator for Doctor
        public async Task<IEnumerable<VisitSchedule>> GetAllForDoctorAsync(int doctorId)
        {
            string query = "sp_Get_Schedules_For_Doctor"; // Make sure this matches your stored procedure
            return await _db.GetData<VisitSchedule, dynamic>(query, new { DoctorID = doctorId });
        }


    }
}
