using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository.Nusrse;
using WardDapperMVC.Models.Domain.Nurse;

namespace WardManagementSystem.Controllers.Nurse
{
    public class DoctorAdviceController : Controller
    {
        private readonly IDoctorAdviceRepo _DrAdvRepo;

        public DoctorAdviceController(IDoctorAdviceRepo doctorAdviceRepo)
        {
            _DrAdvRepo = doctorAdviceRepo;
        }
        public async Task<IActionResult> AddRecord()
        {
            return View("~/Views/Nurse/DoctorAdvice/AddRecord.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(DoctorAdvice doctorAdvice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Nurse/DoctorAdvice/AddRecord.cshtml",doctorAdvice);
                }
                bool addRecordResult = await _DrAdvRepo.AddDocAdviceAsync(doctorAdvice);

                if (addRecordResult)
                {
                    TempData["msg"] = "Successfully recorded.";
                }
                else
                {
                    TempData["msg"] = "Failed to record Dr Advice.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Failed to record Dr Advice.";
            }
            return RedirectToAction(nameof(AddRecord));
        }

        public async Task<IActionResult> DisplayAllRecords()
        {
            var advice = await _DrAdvRepo.GetAllDocAdviceAsync();
            return View("~/Views/Nurse/DoctorAdvice/DisplayAllRecords.cshtml",advice);
        }
        public async Task<IActionResult> DisplayInstructions()
        {
            var instructions = await _DrAdvRepo.GetInstructionsAsync();
            return View("~/Views/Nurse/DoctorAdvice/DisplayInstructions.cshtml", instructions);
        }

        //[HttpPost("DoctorAdvice/ViewDocAdvice/{id}")]
        //public async Task<IActionResult> ViewDocAdvice(int id)
        //{
        //    var viewRecord = await _DrAdvRepo.GetDocAdviceByIdAsync(id);
        //    if (viewRecord == null)
        //    {
        //        return NotFound(); // Handle the case where the record isn't found
        //    }
        //    return RedirectToAction(nameof(DisplayAllRecords));
        //}

        [HttpGet("DoctorAdvice/ViewDocAdvice")]
        public async Task<IActionResult> ViewDocAdvice(int id)
        {
            var viewRecord = await _DrAdvRepo.GetDocAdviceByIdAsync(id);
            if (viewRecord == null)
            {
                return NotFound(); // Handle the case where the record isn't found
            }
            return View("~/Views/Nurse/DoctorAdvice/ViewDocAdvice.cshtml", viewRecord);
        }

        [HttpGet("DoctorAdvice/ViewDocInt")]
        public async Task<IActionResult> ViewDocInt(int id)
        {
            var viewRecord = await _DrAdvRepo.GetDocInstructionByIdAsync(id);
            if (viewRecord == null)
            {
                return NotFound(); // Handle the case where the record isn't found
            }
            return View("~/Views/Nurse/DoctorAdvice/ViewDocInt.cshtml", viewRecord);
        }

        public async Task<IActionResult> UpdateRecord(int id)
        {
            var record = await _DrAdvRepo.GetDocAdviceByIdAsync(id);
            return View("~/Views/Nurse/DoctorAdvice/UpdateRecord.cshtml",record);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecord(DoctorAdvice doctorAdvice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Nurse/DoctorAdvice/UpdateRecord.cshtml",doctorAdvice);
                }
                var updateResult = await _DrAdvRepo.UpdateDocAdviceAsync(doctorAdvice);

                if (updateResult)
                {
                    TempData["msg"] = "Successfully updated.";
                }
                else
                {
                    TempData["msg"] = "Failed to update.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Failed to update.";
            }
            return RedirectToAction(nameof(doctorAdvice));
        }

        public async Task<IActionResult> DeleteRecord(int id)
        {
            var deleteResult = await _DrAdvRepo.DeleteDocAdviceAsync(id);
            return RedirectToAction(nameof(DisplayAllRecords));
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            // Check if the searchTerm is empty
            if (string.IsNullOrEmpty(searchTerm))
            {
                ModelState.AddModelError("SearchError", "Please enter a valid Patient Number.");

                // Retrieve all vitals and return them along with the error message
                var allVitals = await _DrAdvRepo.GetAllDocAdviceAsync();
                return View("~/Views/Nurse/DoctorAdvice/DisplayAllRecords.cshtml", allVitals);
            }

            // Perform the search based on the PatientNumber (string match)
            var vitals = await _DrAdvRepo.GetAllDocAdviceAsync();
            var filteredVitals = vitals.Where(v => v.PatientNumber != null && v.PatientNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Return the filtered vitals to the same view
            return View("~/Views/Nurse/DoctorAdvice/DisplayAllRecords.cshtml", filteredVitals);
        }

        public async Task<IActionResult> SearchInstruction(string searchTerm)
        {
            // Check if the searchTerm is empty
            if (string.IsNullOrEmpty(searchTerm))
            {
                ModelState.AddModelError("SearchError", "Please enter a valid Patient Number.");

                // Retrieve all vitals and return them along with the error message
                var allVitals = await _DrAdvRepo.GetInstructionsAsync();
                return View("~/Views/Nurse/DoctorAdvice/DisplayInstructions.cshtml", allVitals);
            }

            // Perform the search based on the PatientNumber (string match)
            var vitals = await _DrAdvRepo.GetInstructionsAsync();
            var filteredVitals = vitals.Where(v => v.PatientNumber != null && v.PatientNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Return the filtered vitals to the same view
            return View("~/Views/Nurse/DoctorAdvice/DisplayInstructions.cshtml", filteredVitals);
        }

    }
}
