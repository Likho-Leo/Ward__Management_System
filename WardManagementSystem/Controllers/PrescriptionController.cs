using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;
using System.Threading.Tasks;

namespace WardManagementSystem.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionRepository _repo;

        public PrescriptionController(IPrescriptionRepository prescriptionRepository)
        {
            _repo = prescriptionRepository;
        }

        //This is for geting Patient Details using PatientNumber
        [HttpGet]
        public async Task<IActionResult> GetPatientByNumber(string patientNumber)
        {
            if (string.IsNullOrEmpty(patientNumber))
            {
                return Json(null);
            }

            var patient = await _repo.GetPatientByNumberAsync(patientNumber);
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

        //This is for geting employee Details using PatientNumber
        [HttpGet]
        public async Task<IActionResult> GetEmployeeByNumber(string employeeNumber)
        {
            if (string.IsNullOrEmpty(employeeNumber))
            {
                return Json(null);
            }

            var employee = await _repo.GetEmplyeeByNumberAsync(employeeNumber);
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

        public async Task<IActionResult> DisplayAll()
        {
            var prescriptions = await _repo.GetAllAsync();
            return View(prescriptions);
        }

        public IActionResult Add()
        {
            return View(); // or simply return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Prescription model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                bool addScript = await _repo.AddAsync(model); // Pass the ViewModel to the repository
                if (addScript)
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
            var script = await _repo.GetByIdAsync(id);
            return View(script);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Prescription prescription)
        {
            if (!ModelState.IsValid)
                return View(prescription);

            var result = await _repo.UpdateAsync(prescription);
            if (result)
                TempData["msg"] = "Prescription updated successfully.";
            else
                TempData["msg"] = "Error updating prescription.";

            return RedirectToAction(nameof(DisplayAll));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var prescription = await _repo.GetByIdAsync(id);
            return View(prescription);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, Prescription prescription)
        {
            var result = await _repo.DeleteAsync(id);
            if (result)
                TempData["msg"] = "Prescription deleted successfully.";
            else
                TempData["msg"] = "Error deleting prescription.";

            return RedirectToAction(nameof(DisplayAll));
        }


        public async Task<IActionResult> Details(int id)
        {
            var prescription = await _repo.GetPrescriptionDetailsAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            return View(prescription);
        }

    }
}