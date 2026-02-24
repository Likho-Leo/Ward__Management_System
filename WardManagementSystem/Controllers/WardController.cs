using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class WardController : Controller
    {
        private readonly IWardRepository _wardRepository;

        public WardController(IWardRepository wardRepository)
        {
            _wardRepository = wardRepository;
        }

        [HttpGet]
        public IActionResult AddWard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWard(Ward ward)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(ward);

                bool addResults = await _wardRepository.AddWardAsync(ward);

                if (addResults)
                {
                    TempData["msg"] = "Ward information has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add ward information. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!"+ ex.Message;
            }
            return RedirectToAction(nameof(AddWard));
        }

        [HttpGet]
        public async Task<IActionResult> EditWard(int id)
        {
            // Retrieve patient from database or other data source based on id
            var result = await _wardRepository.GetWardByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditWard(Ward ward)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(ward);

                bool update = await _wardRepository.UpdateWardAsync(ward);

                if (update)
                    TempData["msg"] = "Ward information has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!"+ ex.Message;
            }

            return RedirectToAction(nameof(DisplayAllWard));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllWard(string? search)
        {
            var results = await _wardRepository.GetAllWardsAsync();
            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(p => p.WardName.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(results);
        }

        public async Task<IActionResult> DeleteWard(int id)
        {
            var deleteResult = await _wardRepository.DeleteWardAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Ward information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Ward information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllWard));
        }

        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
