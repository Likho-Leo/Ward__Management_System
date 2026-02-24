using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> DisplayNewRefers()
        {
            var patients = await _patientRepository.GetAllNewRefersAsync();
            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> AddPatient(int? id, string firstName, string lastName)
        {
            var patient = new Patient();

            if (id.HasValue)
            {
                var referral = await _patientRepository.GetReferralByIdAsync(id.Value);
                if (referral != null)
                {
                    patient.ReferPatientID = referral.ReferPatientID;
                    patient.FirstName = firstName;
                    patient.LastName = lastName;
                    patient.DOB = referral.Date;
                }
            }

            // Generate PatientNumber
            var lastPatient = await _patientRepository.GetLastPatientAsync();
            string newPatientNumber;

            if (lastPatient != null)
            {
                int lastNumber = int.Parse(lastPatient.PatientNumber[3..]);
                newPatientNumber = $"PAT{lastNumber + 1:D2}";
            }
            else
            {
                newPatientNumber = "PAT01"; // Starting value
            }

            patient.PatientNumber = newPatientNumber; // Set the new patient number

            // Check for new referrals
            var newReferralCount = await _patientRepository.GetNewReferralCountAsync(); // Implement this method in your repository

            // Store count in ViewData
            ViewData["NewReferralCount"] = newReferralCount;

            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(Patient patient, int? ReferPatientID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _patientRepository.AddPatientAsync(patient);
                    if (result)
                    {
                        if (ReferPatientID.HasValue)
                        {
                            await _patientRepository.UpdateReferralStatusAsync(ReferPatientID.Value, "Admitted");
                        }

                        SetTempDataMessage("Patient admitted successfully.");
                        return RedirectToAction("DisplayAllPatient");
                    }
                    ModelState.AddModelError("", "Patient could not be admitted. Please check the details and try again or contact the administrator.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred: " + ex.Message);
                }
            }
            return View(patient);
        }


        [HttpGet]
        public async Task<IActionResult> EditPatient(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            // Check for new referrals
            var newReferralCount = await _patientRepository.GetNewReferralCountAsync(); // Implement this method in your repository

            // Store count in ViewData
            ViewData["NewReferralCount"] = newReferralCount;

            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool updateRecord = await _patientRepository.UpdatePatientAsync(patient);
                    if (updateRecord)
                    {
                        TempData["msg"] = "Patient information updated successfully.";
                        return RedirectToAction("DisplayAllPatient"); // Redirect to DisplayAllPatient
                    }
                    TempData["msg"] = "Patient information could not be updated. Please check the details and try again or contact the administrator.";
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Something went wrong: " + ex.Message;
                }
            }
            return View(patient);
        }


        public async Task<IActionResult> DisplayAllPatient(string? search)
        {
            var patients = await _patientRepository.GetAllPatientsAsync();

            // Check for new referrals
            var newReferralCount = await _patientRepository.GetNewReferralCountAsync(); // Implement this method in your repository

            // Store count in ViewData
            ViewData["NewReferralCount"] = newReferralCount;

            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                patients = patients.Where(p => p.PatientNumber.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(patients);
        }

        public async Task<IActionResult> TrackVisit(string? search)
        {
            var patients = await _patientRepository.TrackVisit();

            // Check for new referrals
            var newReferralCount = await _patientRepository.GetNewReferralCountAsync(); // Implement this method in your repository

            // Store count in ViewData
            ViewData["NewReferralCount"] = newReferralCount;

            return View(patients);
        }

        [HttpPost] // Change to POST to avoid accidental deletes via GET
        public async Task<IActionResult> DeletePatient(int id)
        {
            var deleteResult = await _patientRepository.DeletePatientAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Patient deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Patient could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllPatient));
        }

        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
