using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class MedicationController : Controller
    {
        private readonly IMedicationRepository _medicationRepository;

        public MedicationController(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        [HttpGet]
        public IActionResult AddMed()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMed(Medication medication)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(medication);
                bool addMed = await _medicationRepository.AddMedicationAsync(medication);

                if (addMed)
                {
                    TempData["msg"] = "Medication has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add medication. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!" + ex.Message;
            }
            return RedirectToAction(nameof(AddMed));
        }

        [HttpGet]
        public async Task<IActionResult> EditMed(int id)
        {
            // Retrieve patient from database or other data source based on id
            var Med = await _medicationRepository.GetMedicationByIdAsync(id);

            if (Med == null)
            {
                return NotFound(); 
            }

            return View(Med);
        }

        [HttpPost]
        public async Task<IActionResult> EditMed(Medication medication)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(medication);
                bool updateMed = await _medicationRepository.UpdateMedicationAsync(medication);

                if (updateMed)
                    TempData["msg"] = "Medication has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!" + ex.Message;
            }
            return RedirectToAction(nameof(DisplayAllMed));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllMed(string? search)
        {
            var medications = await _medicationRepository.GetAllMedicationsAsync();
            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                medications = medications.Where(p => p.MedicationName.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(medications);
        }

        public async Task<IActionResult> DeleteMed(int id)
        {
            var deleteResult = await _medicationRepository.DeleteMedicatonAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Medication information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Medication information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllMed));
        }
        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
