using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Model.Domain;

namespace WardManagementSystem.Controllers
{
    public class InstructionController : Controller
    {
        private readonly IPatientInstructionRepository _repo;

        public InstructionController(IPatientInstructionRepository instructionRepository)
        {
            _repo = instructionRepository;
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PatientInstruction patient)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(patient);
                bool addInstruction = await _repo.AddAsync(patient);
                if (addInstruction)
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
                TempData["msg"] = "Something went wrong!";
            }
            return RedirectToAction(nameof(DisplayAll));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var instruction = await _repo.GetByIdAsync(id);
            if (instruction == null)
            {
                return NotFound();
            }
            return View(instruction); // This sends the instruction object to the view
        }


        [HttpPost]
        public async Task<IActionResult> Edit(PatientInstruction patient)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(patient);
                bool updateRecord = await _repo.UpdateAsync(patient);
                if (updateRecord)
                    TempData["msg"] = "Successfully Updated";
                else
                    TempData["msg"] = "Could not update";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!";
            }
            return RedirectToAction(nameof(DisplayAll));
        }

        public async Task<IActionResult> DisplayAll()
        {
            var instructions = await _repo.GetAllAsync();
            return View(instructions);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var instruction = await _repo.GetByIdAsync(id);
            if (instruction == null)
            {
                return NotFound();
            }
            return View(instruction);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, PatientInstruction patient,VisitSchedule schedule)
        {
            var deleteResult = await _repo.DeleteAsync(id);
            if (deleteResult)
            {
                TempData["msg"] = "Successfully Deleted";
            }
            else
            {
                TempData["msg"] = "Could not delete";
            }
            return RedirectToAction(nameof(DisplayAll));
        }

    }
}
