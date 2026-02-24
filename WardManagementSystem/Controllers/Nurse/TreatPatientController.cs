using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository.Nusrse;
using WardDapperMVC.Models.Domain.Nurse;

namespace WardManagementSystem.Controllers.Nurse
{
    public class TreatPatientController : Controller
    {
        private readonly ITreatPatientRepo _treatPatientRepo;

        public TreatPatientController(ITreatPatientRepo treatPatientRepo)
        {
            _treatPatientRepo = treatPatientRepo;
        }
        public async Task<IActionResult> AddRecord()
        {
            return View("~/Views/Nurse/TreatPatient/AddRecord.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(TreatPatient treatPatient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Nurse/TreatPatient/AddRecord.cshtml", treatPatient);
                }
                bool addTreatmentResult = await _treatPatientRepo.AddTreatmentAsync(treatPatient);

                TempData["msg"] = addTreatmentResult? "Successfully recorded" : "Failed to record patient treatment.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "An error occurred: " + ex.Message;
            }
            return RedirectToAction(nameof(AddRecord));
        }

        public async Task<IActionResult> UpdateRecord(int id)
        {
            var treatment = await _treatPatientRepo.GetTreatmentByIdAsync(id);

            if (treatment == null)
            {
                return NotFound(); // Handle the case where the record is not found
            }

            return View("~/Views/Nurse/TreatPatient/UpdateRecord.cshtml", treatment);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecord(TreatPatient treatPatient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Nurse/TreatPatient/UpdateRecord.cshtml", treatPatient);
                }

                var updateResult = await _treatPatientRepo.UpdateTreatmentAsync(treatPatient);
                if (updateResult)
                {
                    TempData["msg"] = "Successfully Updated.";
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
            return View("~/Views/Nurse/TreatPatient/UpdateRecord.cshtml", treatPatient);
        }
        public async Task<IActionResult> DisplayAllRecords()
        {
            var records = await _treatPatientRepo.GetAllTreatmentsAsync();
            return View("~/Views/Nurse/TreatPatient/DisplayAllRecords.cshtml", records);
        }

        //public async Task<IActionResult> DeleteRecord(int id)
        //{
        //    var deleteResult = await _treatPatientRepo.DeleteTreatmentAsync(id);
        //    return RedirectToAction(nameof(DisplayAllRecords));
        //}

        [HttpGet("TreatPatient/ViewDeleteTreatment")]
        public async Task<IActionResult> ViewDeleteTreatment(int id)
        {
            var vital = await _treatPatientRepo.GetTreatmentByIdAsync(id);
            if (vital == null)
            {
                return NotFound(); // Handle the case where the record is not found
            }
            return View("~/Views/Nurse/TreatPatient/DeleteRecord.cshtml", vital);
        }

        [HttpPost("TreatPatient/DeleteTreatment/{id}")]
        public async Task<IActionResult> DeleteTreatment(int id)
        {
            var deleteVital = await _treatPatientRepo.DeleteTreatmentAsync(id);
            if (deleteVital == null)
            {
                return NotFound(); // Handle the case where the record isn't found
            }
            return RedirectToAction(nameof(DisplayAllRecords));
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
                var allVitals = await _treatPatientRepo.GetAllTreatmentsAsync();
                return View("~/Views/Nurse/TreatPatient/DisplayAllRecords.cshtml", allVitals);
            }

            // Perform the search based on the PatientNumber (string match)
            var vitals = await _treatPatientRepo.GetAllTreatmentsAsync();
            var filteredVitals = vitals.Where(v => v.PatientNumber != null && v.PatientNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Return the filtered vitals to the same view
            return View("~/Views/Nurse/TreatPatient/DisplayAllRecords.cshtml", filteredVitals);
        }
    }
}
