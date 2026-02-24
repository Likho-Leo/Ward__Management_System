using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class PatientMovementController : Controller
    {
        private readonly IPatientMovementRepository _patientMovement;
        //For working with bed and ward details
        private readonly IBedRepository _bedRepository;
        private readonly IWardRepository _wardRepository;
        private readonly IPatientRepository _patientRepository;

        public PatientMovementController(IPatientMovementRepository patientMovement,IPatientRepository patientRepository, IBedRepository bedRepository, IWardRepository wardRepository)
        {
            _patientMovement = patientMovement;
            _bedRepository = bedRepository;
            _wardRepository = wardRepository;
            _patientRepository = patientRepository;
        }

        //This is for geting Patient Details using PatientNumber
        [HttpGet]
        public async Task<IActionResult> GetPatientByNumber(string patientNumber)
        {
            if (string.IsNullOrEmpty(patientNumber))
            {
                return Json(null);
            }

            var patient = await _patientMovement.GetPatientByNumberAsync(patientNumber);
            if (patient == null)
            {
                return Json(null);
            }

            var patientInfo = new
            {
                firstName = patient.FirstName,
                lastName = patient.LastName,
                patientID = patient.PatientId
            };

            return Json(patientInfo);
        }

        //This is for changing bed availability status
        [HttpPost]
        public async Task<IActionResult> UpdateBedAvailability(int newBedID, int? previousBedID)
        {
            if (previousBedID.HasValue)
            {
                // Update previous bed to available
                await _bedRepository.UpdateBedAvailabilityAsync(previousBedID.Value, "Available");
            }

            // Update new bed to not available
            await _bedRepository.UpdateBedAvailabilityAsync(newBedID, "Not Available");

            return Ok();
        }

        //handle the AJAX request for getting beds by ward
        [HttpGet]
        public async Task<IActionResult> GetBedsByWard(int wardId)
        {
            if (wardId <= 0)
            {
                return Json(new List<SelectListItem>());
            }

            //For working Bed to do the drop down beside on the selected ward
            var beds = await _bedRepository.GetBedsByWardAsync(wardId);
            var bedList = beds.Select(b => new { value = b.BedID, text = b.BedNo }).ToList();
            return Json(bedList);
        }


        //Code below is for Adding
        [HttpGet]
        public async Task<IActionResult> AddMovement()
        {
            //For working Ward to do the drop down
            var wards = await _wardRepository.GetAllWardsAsync();
            ViewBag.WardList = new SelectList(wards, "WardID", "WardName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMovement(PatientMovement patientMovement)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //For working Ward to do the drop down if it's not adding or validated
                    var wards = await _wardRepository.GetAllWardsAsync();
                    ViewBag.WardList = new SelectList(wards, "WardID", "WardName");

                    return View(patientMovement);
                }

                bool addResults = await _patientMovement.AddPatientMovementAsync(patientMovement);

                // Update bed availability status automatical
                if (patientMovement.BedID > 0)
                {
                    await _bedRepository.UpdateBedAvailabilityAsync(patientMovement.BedID, "Not Available");
                }

                if (addResults)
                {
                    TempData["msg"] = "Patient has been successfully moved.";
                }
                else
                {
                    TempData["msg"] = "Failed to move patient. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }

            return RedirectToAction(nameof(AddMovement));
        }

        //Code below is for editing
        [HttpGet]
        public async Task<IActionResult> EditMovement(int id)
        {
            // Retrieve patient from database or other data source based on id
            var results = await _patientMovement.GetPatientMovementByIdAsync(id);

            if (results == null)
            {
                return NotFound();
            }

            // Retrieve wards and beds for dropdowns
            var wards = await _wardRepository.GetAllWardsAsync();
            var beds = await _bedRepository.GetAllBedByAvailabiliyiStatus();

            // Set ViewBag properties
            ViewBag.PatientFolder = results;
            ViewBag.WardList = new SelectList(wards, "WardID", "WardName");
            ViewBag.BedList = new SelectList(beds, "BedID", "BedNo");
            ViewBag.PreviousBedID = results.BedID;

            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovement(PatientMovement patientMovement)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Prepare ViewBag again with updated dropdowns
                    var wards = await _wardRepository.GetAllWardsAsync();
                    var beds = await _bedRepository.GetAllBedByAvailabiliyiStatus();

                    ViewBag.PatientFolder = patientMovement;
                    ViewBag.WardList = new SelectList(wards, "WardID", "WardName");
                    ViewBag.BedList = new SelectList(beds, "BedID", "BedNo");
                    ViewBag.PreviousBedID = patientMovement.PreviousBedID;

                    return View(patientMovement);
                }

                // Fetch current patient Movement from repository
                var currentMovement = await _patientMovement.GetPatientMovementByIdAsync(patientMovement.MovementID);

                bool updateRecord = await _patientMovement.UpdatePatientMovementAsync(patientMovement);

                if (updateRecord)
                {
                    if (patientMovement.BedID != currentMovement.BedID)
                    {
                        // Update new bed to Not Available
                        if (patientMovement.BedID > 0)
                        {
                            await _bedRepository.UpdateBedAvailabilityAsync(patientMovement.BedID, "Not Available");
                        }

                        // Update old bed to Available
                        if (currentMovement.BedID > 0)
                        {
                            await _bedRepository.UpdateBedAvailabilityAsync(currentMovement.BedID, "Available");
                        }
                    }

                    //Update bed to Available when the user check out
                    if (patientMovement.Status != currentMovement.Status)
                    {
                        if (patientMovement.Status == "Check-in")
                        {
                            await _bedRepository.UpdateBedAvailabilityAsync(patientMovement.BedID, "Not Available");
                        }

                        if (patientMovement.Status == "Check-out")
                        {
                            await _bedRepository.UpdateBedAvailabilityAsync(currentMovement.BedID, "Available");
                        }
                    }

                    TempData["msg"] = "Patient movement has been successfully updated.";
                }
                else
                {
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }
            return RedirectToAction(nameof(DisplayAllMovements));
        }

        //THIS CODE BELOW IS FOR DISPLAYING ALL MOVEMENT
        [HttpGet]
        public async Task<IActionResult> DisplayAllMovements(string? search)
        {
            var results = await _patientMovement.GetAllPatientMovementsAsync();

            // Check for new referrals
            var newReferralCount = await _patientRepository.GetNewReferralCountAsync(); // Implement this method in your repository

            // Store count in ViewData
            ViewData["NewReferralCount"] = newReferralCount;

            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(p => p.PatientNumber.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(results);
        }

        //THIS CODE BELOW IS FOR DELETING ALL MOVEMENT
        public async Task<IActionResult> DeleteMovement(int id)
        {
            var deleteResult = await _patientMovement.DeletePatientMovementAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Patient Movement information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Patient Movement information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllMovements));
        }

        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
