using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class AllergyController : Controller
    {
        private readonly IAllergyRepository _allergyRepository;

        public AllergyController(IAllergyRepository allergyRepository)
        {
            _allergyRepository = allergyRepository;
        }

        [HttpGet]
        public IActionResult AddAllergy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAllergy(Allergy allergy)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(allergy);

                bool addResults = await _allergyRepository.AddAllergyAsync(allergy);

                if (addResults)
                {
                    TempData["msg"] = "Allergy information has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add allergy information. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!" + ex.Message;
            }
            return RedirectToAction(nameof(AddAllergy));
        }

        [HttpGet]
        public async Task<IActionResult> EditAllergy(int id)
        {
            // Retrieve patient from database or other data source based on id
            var result = await _allergyRepository.GetAllergyByIdAsync(id);

            if (result == null)
            {
                return NotFound(); 
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditAllergy(Allergy allergy)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(allergy);

                bool update = await _allergyRepository.UpdateAllergyAsync(allergy);

                if (update)
                    TempData["msg"] = "Allergy information has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!"+ ex.Message;
            }
            return RedirectToAction(nameof(DisplayAllAllergy));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllAllergy(string? search)
        {
            var results = await _allergyRepository.GetAllAllergiesAsync();
            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(p => p.Allergen.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(results);
        }
        public async Task<IActionResult> DeleteAllergy(int id)
        {
            var deleteResult = await _allergyRepository.DeleteAllergyAsync(id);
            return RedirectToAction(nameof(DisplayAllAllergy));
        }
    }
}
