using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class HospitalInformationController : Controller
    {
        private readonly IHospitalInformationRepository _hospitalInformationRepository;

        public HospitalInformationController(IHospitalInformationRepository hospitalInformationRepository)
        {
            _hospitalInformationRepository = hospitalInformationRepository;
        }

        //New Methods

        /* This method is for gething the Logo and display it when the use clicked Edit button */
        [HttpGet]
        public async Task<IActionResult> GetLogo(int id)
        {
            var hospital = await _hospitalInformationRepository.GetHospitalInfoByIdAsync(id);

            if (hospital == null || hospital.Logo == null || hospital.Logo.Length == 0)
            {
                return NotFound();
            }

            return File(hospital.Logo, "image/jpeg"); // Assuming the logo is JPEG; adjust MIME type as needed
        }

        //The one we all know
        [HttpGet]
        public IActionResult AddHospital()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHospital(HospitalInformation hospital)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(hospital);

                //This is for converting the logo image to byte
                if (hospital.LogoFile != null && hospital.LogoFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await hospital.LogoFile.CopyToAsync(memoryStream);
                    hospital.Logo = memoryStream.ToArray(); // Convert uploaded file to byte array
                }

                bool addResults = await _hospitalInformationRepository.AddHospitalInfoAsync(hospital);

                if (addResults)
                {
                    TempData["msg"] = "Hospital information has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add hospital information. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }
            return RedirectToAction(nameof(AddHospital));
        }

        public async Task<IActionResult> EditHospital(int id)
        {
            // Retrieve patient from database or other data source based on id
            var result = await _hospitalInformationRepository.GetHospitalInfoByIdAsync(id);

            if (result == null)
            {
                return NotFound(); 
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditHospital(HospitalInformation hospital)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Re-fetch the existing hospital info to maintain the current logo in case of validation failure
                    var existingHospitalD = await _hospitalInformationRepository.GetHospitalInfoByIdAsync(hospital.InfoID);
                    if (existingHospitalD != null)
                    {
                        hospital.Logo = existingHospitalD.Logo;
                    }

                    // Return the view with the current state of the model including the existing logo
                    return View(hospital);
                }

                // Retrieve existing hospital information
                var existingHospital = await _hospitalInformationRepository.GetHospitalInfoByIdAsync(hospital.InfoID);

                if (hospital.LogoFile != null && hospital.LogoFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await hospital.LogoFile.CopyToAsync(memoryStream);
                    hospital.Logo = memoryStream.ToArray(); // Convert uploaded file to byte array
                }
                else
                {
                    // Preserve the existing logo if no new file is uploaded
                    hospital.Logo = existingHospital.Logo;
                }

                bool update = await _hospitalInformationRepository.UpdateHospitalInfoAsync(hospital);

                if (update)
                    TempData["msg"] = "Hospital information has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!" + ex.Message;
            }
            return RedirectToAction(nameof(DisplayAllHospital));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllHospital()
        {
            var results = await _hospitalInformationRepository.GetAllHospitalInfoAsync();
            return View(results);
        }

        public async Task<IActionResult> DeletHospital(int id)
        {
            var deleteResult = await _hospitalInformationRepository.DeleteHospitalInfoAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Hospital information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Hospital information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllHospital));
        }
        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
