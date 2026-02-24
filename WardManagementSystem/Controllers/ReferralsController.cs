using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class ReferralsController : Controller
    {
        private readonly IPatientReferralsRepository _repo;

        public ReferralsController(IPatientReferralsRepository referralsRepository)
        {
            _repo = referralsRepository;
        }

        //This is for geting employee Details using PatientNumber
        [HttpGet]
        public async Task<IActionResult> GetEmployeeByNumber(string employeeNumber)
        {
            if (string.IsNullOrEmpty(employeeNumber))
            {
                return Json(null);
            }

            var employee = await _repo.GetEmployeeByNumberAsync(employeeNumber);
            if (employee == null)
            {
                return Json(null);
            }

            var patientInfo = new
            {
                firstName = employee.FirstName,
                lastName = employee.LastName,
                userID = employee.UserID
            };

            return Json(patientInfo);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PatientReferrals model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                bool addReferral = await _repo.AddAsync(model); // Pass the ViewModel to the repository
                if (addReferral)
                {
                    TempData["msg"] = "Successfully Added";
                }
                else
                {
                    TempData["msg"] = "Could not add";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!";
            }
            return RedirectToAction(nameof(DisplayAll));
        }




        public async Task<IActionResult> Edit(int id)
        {
            var referrals = await _repo.GetByIdAsync(id);
            return View(referrals);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PatientReferrals referral)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(referral);
                bool updateRecord = await _repo.UpdateAsync(referral);

                if (updateRecord)
                    TempData["msg"] = "Successfully Added";
                else
                    TempData["msg"] = "Oh Hell Nah";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Seriously!!!!!";
            }
            return RedirectToAction(nameof(DisplayAll));
        }
        public async Task<IActionResult> DisplayAll()
        {
            var patients = await _repo.GetAllAsync();
            return View(patients);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var referral = await _repo.GetByIdAsync(id);

            if (referral == null)
            {
                return NotFound(); // Handle not found
            }

            return View(referral); // Pass the referral object to the view
        }


        public async Task<IActionResult> Details(int id)
        {
            var referral = await _repo.GetReferralDetailsAsync(id);

            if (referral == null)
            {
                return NotFound();
            }

            return View(referral);
        }



    }
}