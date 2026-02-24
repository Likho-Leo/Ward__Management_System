using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class PatientFolderController : Controller
    {
        private readonly IPatientFolderRepository _folderRepository;
        //For working with bed, discharge and ward details
        private readonly IBedRepository _bedRepository;
        private readonly IWardRepository _wardRepository;
        private readonly IDischargePatientRepository _dischargeRepository;
        private readonly IPatientRepository _patientRepository;

        public PatientFolderController(IPatientFolderRepository folderRepository, IPatientRepository patientRepository, IBedRepository bedRepository, IWardRepository wardRepository, IDischargePatientRepository dischargeRepository)
        {
            _folderRepository = folderRepository;
            //For working with bed, discharge and ward details
            _bedRepository = bedRepository;
            _wardRepository = wardRepository;
            _dischargeRepository = dischargeRepository;
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

            var patient = await _folderRepository.GetPatientByNumberAsync(patientNumber);
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


        [HttpGet]
        public async Task<IActionResult> AddPatientFolder()
        {
            //For working Ward to do the drop down
            var wards = await _wardRepository.GetAllWardsAsync();
            ViewBag.WardList = new SelectList(wards, "WardID", "WardName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientFolder(PatientFolder patientFolder)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //For working Ward to do the drop down if it's not adding or validated
                    var wards = await _wardRepository.GetAllWardsAsync();
                    ViewBag.WardList = new SelectList(wards, "WardID", "WardName");

                    return View(patientFolder);
                }

                bool addResults = await _folderRepository.AddPatientFolderAsync(patientFolder);

                // Update bed availability status automatical
                if (patientFolder.BedID > 0)
                {
                    await _bedRepository.UpdateBedAvailabilityAsync(patientFolder.BedID, "Not Available");
                }

                if (addResults)
                {
                    TempData["msg"] = "The patient folder has been successfully opened.";
                }
                else
                {
                    TempData["msg"] = "Patient folder could not be opened. Please check the details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }

            return RedirectToAction(nameof(AddPatientFolder));
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientFolder(int id)
        {
            // Retrieve patient from database or other data source based on id
            var results = await _folderRepository.GetPatientFolderByIdAsync(id);

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
        public async Task<IActionResult> EditPatientFolder(PatientFolder patientFolder)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Prepare ViewBag again with updated dropdowns
                    var wards = await _wardRepository.GetAllWardsAsync();
                    var beds = await _bedRepository.GetAllBedByAvailabiliyiStatus();

                    ViewBag.PatientFolder = patientFolder;
                    ViewBag.WardList = new SelectList(wards, "WardID", "WardName");
                    ViewBag.BedList = new SelectList(beds, "BedID", "BedNo");
                    ViewBag.PreviousBedID = patientFolder.PreviousBedID;

                    return View(patientFolder);
                }

                // Fetch current patient folder from repository
                var currentFolder = await _folderRepository.GetPatientFolderByIdAsync(patientFolder.FolderID);

                bool updateRecord = await _folderRepository.UpdatePatientFolderAsync(patientFolder);

                if (updateRecord)
                {
                    if (patientFolder.BedID != currentFolder.BedID)
                    {
                        // Update new bed to Not Available
                        if (patientFolder.BedID > 0)
                        {
                            await _bedRepository.UpdateBedAvailabilityAsync(patientFolder.BedID, "Not Available");
                        }

                        // Update old bed to Available
                        if (currentFolder.BedID > 0)
                        {
                            await _bedRepository.UpdateBedAvailabilityAsync(currentFolder.BedID, "Available");
                        }
                    }
                    TempData["msg"] = "Patient folder has been successfully updated.";
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
            return RedirectToAction(nameof(DisplayAllPatientFolder));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllPatientFolder(string? search)
        {
            var results = await _folderRepository.GetAllPatientFoldersAsync();

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

        //if we want to use delete
        public async Task<IActionResult> DeletePatientFolder(int id)
        {
            var deleteResult = await _folderRepository.DeletePatientFolderAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Patient Folder information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Patient Folder information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllPatientFolder));
        }
        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }

        //This is for discharging patients
        [HttpGet]
        public async Task<IActionResult> DischargePatient(int id)
        {
            var patientFolder = await _folderRepository.GetPatientFolderByIdAsync(id);

            if (patientFolder == null)
            {
                return NotFound();
            }

            // Pass current date as DischargeDate
            var dischargeModel = new DischargePatient
            {
                FolderID = patientFolder.FolderID,
                DischargeDate = DateTime.Now // Current date
            };

            // Pass the discharge model to the view
            ViewBag.DischargeModel = dischargeModel;

            // Check for new referrals
            var newReferralCount = await _patientRepository.GetNewReferralCountAsync(); // Implement this method in your repository

            // Store count in ViewData
            ViewData["NewReferralCount"] = newReferralCount;

            return View(patientFolder);
        }

        [HttpPost]
        public async Task<IActionResult> DischargePatient(PatientFolder patientFolder, DateTime dischargeDate)
        {
            try
            {
                // Update patient folder status to "Discharged"
                patientFolder.Status = "Discharged";
                bool updateResult = await _folderRepository.UpdatePatientFolderAsync(patientFolder);

                if (updateResult)
                {
                    // Update bed availability to "Available"
                    if (patientFolder.BedID > 0)
                    {
                        await _bedRepository.UpdateBedAvailabilityAsync(patientFolder.BedID, "Available");
                    }

                    // Create and save the discharge record
                    var dischargeRecord = new DischargePatient
                    {
                        FolderID = patientFolder.FolderID,
                        DischargeDate = dischargeDate
                    };

                    await _dischargeRepository.AddDischargeRecordAsync(dischargeRecord);

                    //TempData["msg"] = "Patient successfully discharged.";
                }
                else
                {
                    TempData["msg"] = "Patient could not be discharged. Please check the details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong: " + ex.Message;
            }

            return RedirectToAction(nameof(DisplayAllPatientFolder));
        }
    }
}
