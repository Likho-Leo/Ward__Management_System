using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class ConsumableController : Controller
    {
        private readonly IConsumableRepository _consumableRepository;
        //We need these for working with user and manager details
        private readonly IUserRepository _userRepository;
        private readonly IManagerRepository _managerRepository;

        public ConsumableController(IConsumableRepository consumableRepository, IUserRepository userRepository, IManagerRepository managerRepository)
        {
            _consumableRepository = consumableRepository;
            _userRepository = userRepository;
            _managerRepository = managerRepository;
        }

        //New Methods

        /*This method is used to get user details based on the EmployeeNumber that was entered.*/
        [HttpPost]
        public async Task<IActionResult> GetManagerDetails(string employeeNumber)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(employeeNumber))
            {
                return BadRequest(new { message = "A valid Employee Number is required." });
            }

            // Fetch user by employee number
            var user = await _userRepository.GetUserByEmployeeNumberAsync(employeeNumber);
            if (user == null)
            {
                return NotFound(new { message = "User not found. Please check the entered details." });
            }

            // Fetch manager by user ID
            var manager = await _managerRepository.GetManagerByUserIdAsync(user.UserID);
            if (manager == null)
            {
                return NotFound(new { message = "Manager not found. Please check the entered details." });
            }

            // Prepare consumable object
            var consumable = new Consumable
            {
                ManagerID = manager.ManagerID,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            // Return partial view with consumable data
            return PartialView("_ConsumableFormPartial", consumable);
        }



        //The one we know
        [HttpGet]
        public IActionResult AddConsumable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddConsumable(Consumable consumable)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(consumable);
                }

                bool addResults = await _consumableRepository.AddConsumableAsync(consumable);

                if (addResults)
                {
                    TempData["msg"] = "Consumable information has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add consumable information. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }
            return RedirectToAction(nameof(AddConsumable));
        }

        [HttpGet]
        public async Task<IActionResult> EditConsumable(int id)
        {
            // Retrieve patient from database or other data source based on id
            var results = await _consumableRepository.GetConsumableByIdAsync(id);

            if (results == null)
            {
                return NotFound();
            }

            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> EditConsumable(Consumable consumable)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(consumable);
                }

                bool updateRecord = await _consumableRepository.UpdateConsumableAsync(consumable);

                if (updateRecord)
                    TempData["msg"] = "Consumable information has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }

            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }
            return RedirectToAction(nameof(DisplayAllConsumable));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllConsumable(string? search)
        {
            var results = await _consumableRepository.GetAllConsumablesAsync();
            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(p => p.ConsumableType.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(results);
        }

        public async Task<IActionResult> DeleteConsumable(int id)
        {
            var deleteResult = await _consumableRepository.DeleteConsumableAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Consumable information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Consumable information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllConsumable));
        }
        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
