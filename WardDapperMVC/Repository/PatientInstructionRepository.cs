using WardDapperMVC.DataAccess;
using WardDapperMVC.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WardDapperMVC.Repository
{
    public class PatientInstructionRepository : IPatientInstructionRepository
    {
        private readonly ISqlDataAccess _db;

        public PatientInstructionRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(PatientInstruction instruction)
        {
            try
            {
                await _db.SaveData("sp_Create_Instruction", new
                {
                    instruction.PatientID,
                    instruction.DoctorID,
                    instruction.DateOfVisit,
                    instruction.FollowUpAppointmentDate,
                    instruction.DoctorNote,
                    instruction.DischargeStatus,
                    instruction.Rest,
                    instruction.WoundCare,
                    instruction.Medications,
                    instruction.Diet,
                    instruction.EmergencySigns

                });
                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_Delete_Instruction", new { InstructionID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return false;
            }
        }

        public async Task<IEnumerable<PatientInstruction>> GetAllAsync()
        {
            string query = "sp_Get_Instructions";
            return await _db.GetData<PatientInstruction, dynamic>(query, new { });
        }

        public async Task<PatientInstruction> GetByIdAsync(int id)
        {
            var parameters = new { InstructionID = id };
            IEnumerable<PatientInstruction> result = await _db.GetData<PatientInstruction, dynamic>("sp_Get_Instruction", parameters);
            return result.FirstOrDefault();
        }


        public async Task<bool> UpdateAsync(PatientInstruction instruction)
        {
            try
            {
                await _db.SaveData("sp_UpdatePatientInstruction", new
                {
                    InstructionID = instruction.InstructionID,
                    DateOfVisit = instruction.DateOfVisit,
                    FollowUpAppointmentDate = instruction.FollowUpAppointmentDate,
                    Rest = instruction.Rest,
                    WoundCare = instruction.WoundCare,
                    Medications = instruction.Medications,
                    Diet = instruction.Diet,
                    EmergencySigns = instruction.EmergencySigns
                });
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return false;
            }
        }

    }
}
