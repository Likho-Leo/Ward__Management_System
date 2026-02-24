using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository.Nusrse;
using WardDapperMVC.Models.Domain.Nurse;
using Microsoft.AspNetCore.Mvc.Rendering;
using WardDapperMVC.Models.Domain;

namespace WardManagementSystem.Controllers.Nurse
{
    public class MedAdministrationController : Controller
    {
        private readonly IMedAdministrationRepo _medAdminRepo;

        public MedAdministrationController(IMedAdministrationRepo medAdminRepo)
        {
            _medAdminRepo = medAdminRepo;
        }

        public async Task<IActionResult> AddRecord()
        {
            return View("~/Views/Nurse/MedAdministration/AddRecord.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(MedAdministration medAdmin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Nurse/MedAdministration/AddRecord.cshtml", medAdmin);
                }

                bool recMedAdminResult = await _medAdminRepo.AddMedAdminAsync(medAdmin);
                TempData["msg"] = recMedAdminResult ? "Successfully recorded" : "Failed to record medication administration.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction(nameof(AddRecord));
        }


        public async Task<IActionResult> UpdateRecord(int id)
        {
            // Fetch the record you want to update
            var medAdmin = await _medAdminRepo.GetMedAdminById(id);

            if (medAdmin == null)
            {
                return NotFound(); // Handle the case where the record is not found
            }

            // Pass the medAdmin record to the view
            return View("~/Views/Nurse/MedAdministration/UpdateMedAdministered.cshtml", medAdmin);
        }



        [HttpPost]
       public async Task<IActionResult> UpdateRecord(MedAdministration medAdmin)
       {
            try
            {
                if (!ModelState.IsValid)
                {

                    // Return the view with validation errors
                    return View("~/Views/Nurse/MedAdministration/UpdateMedAdministered.cshtml", medAdmin);
                }

                // Proceed with the update if the model is valid
                bool updateResult = await _medAdminRepo.UpdateMedAdminAsync(medAdmin);

                if (updateResult)
                {
                    TempData["msg"] = "Successfully updated.";
                    return View("~/Views/Nurse/MedAdministration/UpdateMedAdministered.cshtml", medAdmin);
                }
                else
                {
                    TempData["msg"] = "Failed to update medication administration.";
                    return View("~/Views/Nurse/MedAdministration/UpdateMedAdministered.cshtml", medAdmin);
                }
            }
            catch (Exception ex)
            {
                // Handle exception and return error message
                TempData["msg"] = "An error occurred: " + ex.Message;
                return View("~/Views/Nurse/MedAdministration/UpdateMedAdministered.cshtml", medAdmin);
            }
       }

       public async Task<IActionResult> DisplayAllRecords()
       {
          var records = await _medAdminRepo.GetAllMedAdminAsync();
          return View("~/Views/Nurse/MedAdministration/DisplayAllRecords.cshtml", records);
       }

        //public async Task<IActionResult> DeleteRecord(int id)
        //{
        //   var deleteResult = await _medAdminRepo.DeleteMedAdminAsync(id);
        //   return RedirectToAction(nameof(DisplayAllRecords));
        //}

        [HttpGet("MedAdministration/ViewDeleteMedAdmin")]
        public async Task<IActionResult> ViewDeleteMedAdmin(int id)
        {
            var vital = await _medAdminRepo.GetMedAdminById(id);
            if (vital == null)
            {
                return NotFound(); // Handle the case where the record is not found
            }
            return View("~/Views/Nurse/MedAdministration/DeleteRecord.cshtml", vital);
        }

        [HttpPost("MedAdministration/DeleteMedAdmin/{id}")]
        public async Task<IActionResult> DeleteMedAdmin(int id)
        {
            var deleteVital = await _medAdminRepo.DeleteMedAdminAsync(id);
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
                var allVitals = await _medAdminRepo.GetAllMedAdminAsync();
                return View("~/Views/Nurse/MedAdministration/DisplayAllRecords.cshtml", allVitals);
            }

            // Perform an exact match search based on patient number (case-insensitive)
            var vitals = await _medAdminRepo.GetAllMedAdminAsync();
            vitals = vitals.Where(v => v.PatientNumber.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Return the filtered vitals to the same view
            return View("~/Views/Nurse/MedAdministration/DisplayAllRecords.cshtml", vitals);
        }

    }
}
