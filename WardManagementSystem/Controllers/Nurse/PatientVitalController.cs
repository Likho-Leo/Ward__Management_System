using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository.Nusrse;
using WardDapperMVC.Models.Domain.Nurse;

namespace WardManagementSystem.Controllers.Nurse
{
    //[Route("Nurse/PatientVital/DisplayAll")]
    public class PatientVitalController : Controller
    {
        private readonly IPatientVitalRepo _patientVitalRepo;

        public PatientVitalController(IPatientVitalRepo patientVitalRepo)
        {
            _patientVitalRepo = patientVitalRepo;
        }
        //get method
        public async Task<IActionResult> RecordVital()
        {
            return View("~/Views/Nurse/PatientVital/RecordVital.cshtml");
        }

        [HttpPost]
        //post method
        public async Task<IActionResult> RecordVital(PatientVital patientVital)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Nurse/PatientVital/RecordVital.cshtml", patientVital);
                }
                bool recordVital = await _patientVitalRepo.CreateRecordAsync(patientVital);
                if (recordVital)
                {
                    TempData["msg"] = "Patient vitals successfully recorded.";
                }
                else
                {
                    TempData["msg"] = "Failed to record patient vitals.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Failed to record patient vitals.";
            }
            return RedirectToAction(nameof(RecordVital));
        }

        [HttpGet("PatientVital/ViewDeleteVital")]
        public async Task<IActionResult> ViewDeleteVital(int id)
        {
            var vital = await _patientVitalRepo.GetRecordByIDAsync(id);
            if (vital == null)
            {
                return NotFound(); // Handle the case where the record is not found
            }
            return View("~/Views/Nurse/PatientVital/DeleteRecord.cshtml", vital);
        }

        [HttpPost("PatientVital/DeleteMedAdmin/{id}")]
        public async Task<IActionResult> DeleteVital(int id)
        {
            var deleteVital = await _patientVitalRepo.DeleteRecordAsync(id);
            if (deleteVital == null)
            {
                return NotFound(); // Handle the case where the record isn't found
            }
            return RedirectToAction(nameof(DisplayAll));
        }


        public async Task<IActionResult> DisplayAll()
        {
            var vitals = await _patientVitalRepo.GetRecordsAsync();
            return View("~/Views/Nurse/PatientVital/DisplayAll.cshtml", vitals);
        }

        public async Task<IActionResult> UpdateVital(int id)
        {
            var vital = _patientVitalRepo.GetRecordByIDAsync(id).Result;

            if (vital == null)
            {
                return NotFound(); // Handle the case where the record is not found
            }

            return View("~/Views/Nurse/PatientVital/UpdateVital.cshtml", vital);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVital(PatientVital patientVital)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Nurse/PatientVital/UpdateVital.cshtml", patientVital);
                }

                var updateSuccess = await _patientVitalRepo.UpdateRecordAsync(patientVital);
                if (updateSuccess)
                {
                    TempData["msg"] = "Patient vital signs updated successfully.";
                    return View("~/Views/Nurse/PatientVital/UpdateVital.cshtml", patientVital);
                }
                else
                {
                    TempData["msg"] = "Failed to update patient treatment.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Failed to update patient treatment.";
            }
            return View("~/Views/Nurse/PatientVital/UpdateVital.cshtml", patientVital);
        }

        // Search action to handle the search functionality
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            // Check if the searchTerm is empty
            if (string.IsNullOrEmpty(searchTerm))
            {
                ModelState.AddModelError("SearchError", "Please enter a valid Patient Number.");

                // Retrieve all vitals and return them along with the error message
                var allVitals = await _patientVitalRepo.GetRecordsAsync();
                return View("~/Views/Nurse/PatientVital/DisplayAll.cshtml", allVitals);
            }

            // Perform an exact match search based on patient number (case-insensitive)
            var vitals = await _patientVitalRepo.GetRecordsAsync();
            vitals = vitals.Where(v => v.PatientNumber.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Return the filtered vitals to the same view
            return View("~/Views/Nurse/PatientVital/DisplayAll.cshtml", vitals);
        }

    }
}

