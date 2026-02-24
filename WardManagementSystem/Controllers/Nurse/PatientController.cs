using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository.Nusrse;
using WardDapperMVC.Models.Domain.Nurse;

namespace WardManagementSystem.Controllers.Nurse
{
    [Route("Patient")]
    public class PatientController : Controller
    {
        private readonly IPatientRepo _patientRepo;

        public PatientController(IPatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        [HttpGet("GetPatientDetailsByNo")]
        public async Task<IActionResult> GetPatientDetailsByNo(string patientNumber)
        {
            var patient = await _patientRepo.GetPatientDetailsByNoAsync(patientNumber);
            if (patient != null)
            {
                var jsonResponse = new
                {
                    success = true,
                    data = new { patientId = patient.PatientId, firstName = patient.FirstName, lastName = patient.LastName }
                };
                Console.WriteLine($"JSON Response: {Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse)}"); // Log JSON response
                return Json(jsonResponse);
            }
            else
            {
                return Json(new { success = false, message = "Patient not found." });
            }


        }


    }

}
