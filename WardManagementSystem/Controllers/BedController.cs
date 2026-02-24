using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class BedController : Controller
    {
        private readonly IBedRepository _bedRepository;
        private readonly IWardRepository _wardRepository; //We need this for working with ward as a dropdown

        public BedController(IBedRepository bedRepository, IWardRepository wardRepository)
        {
            _bedRepository = bedRepository;
            _wardRepository = wardRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AddBed()
        {
            //For populating the ViewBag ward dropdown with details when the user click Add
            var wards = await _wardRepository.GetAllWardsAsync();
            ViewBag.WardList = new SelectList(wards, "WardID", "WardName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBed(Bed bed)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Re-populatinting ViewBag with Ward list 
                    var wards = await _wardRepository.GetAllWardsAsync();
                    ViewBag.WardList = new SelectList(wards, "WardID", "WardName");
                    return View(bed);
                }

                bool addResults = await _bedRepository.AddBedAsync(bed);

                if (addResults)
                {
                    TempData["msg"] = "Bed information has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add bed information. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }
            return RedirectToAction(nameof(AddBed));
        }

        [HttpGet]
        public async Task<IActionResult> EditBed(int id)
        {
   
            // Retrieve patient from database or other data source based on id
            var results = await _bedRepository.GetBedByIdAsync(id);

            if (results == null)
            {
                return NotFound(); 
            }

            // Populate ViewBag with Ward list when the user click Edit
            var wards = await _wardRepository.GetAllWardsAsync();
            ViewBag.WardList = new SelectList(wards, "WardID", "WardName");

            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> EditBed(Bed bed)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Re-populate ViewBag with Ward list
                    var wards = await _wardRepository.GetAllWardsAsync();
                    ViewBag.WardList = new SelectList(wards, "WardID", "WardName");
                    return View(bed);
                }
                    
                bool updateRecord = await _bedRepository.UpdateBedAsync(bed);

                if (updateRecord)
                    TempData["msg"] = "Bed information has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }
            return RedirectToAction(nameof(DisplayAllBed));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllBed(string? search)
        {
            // Fetch all beds asynchronously
            var results = await _bedRepository.GetAllBedsAsync();

            // Filter results if a search term is provided
            if (!string.IsNullOrWhiteSpace(search) && int.TryParse(search.Trim(), out int bedNo))
            {
                results = results
                    .Where(b => b.BedNo == bedNo)
                    .ToList();
            }

            return View(results);
        }

        public async Task<IActionResult> DeleteBed(int id)
        {
            var deleteResult = await _bedRepository.DeleteBedAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Bed information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Bed information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllBed));
        }
        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
