using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class ConditionController : Controller
    {
        private readonly IConditionRepository _conditionRepository;

        public ConditionController(IConditionRepository conditionRepository)
        {
            _conditionRepository = conditionRepository;
        }

        public IActionResult AddCondition()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCondition(Condition condition)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(condition);

                bool addResults = await _conditionRepository.AddConditionAsync(condition);

                if (addResults)
                {
                    TempData["msg"] = "Condition has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add condition. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!" + ex.Message;
            }
            return RedirectToAction(nameof(AddCondition));
        }

        public async Task<IActionResult> EditCondition(int id)
        {
            // Retrieve patient from database or other data source based on id
            var result = await _conditionRepository.GetCondtionByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditCondition(Condition condition)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(condition);

                bool update = await _conditionRepository.UpdateConditionAsync(condition);

                if (update)
                    TempData["msg"] = "Condition has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!" + ex.Message;
            }
            return RedirectToAction(nameof(DisplayAllCond));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllCond(string? search)
        {
            var results = await _conditionRepository.GetAllConditionsAsync();
            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(p => p.Conditions.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(results);
        }

        public async Task<IActionResult> DeleteCondition(int id)
        {
            var deleteResult = await _conditionRepository.DeleteCondtionAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Condition information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Condition information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllCond));
        }
        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
