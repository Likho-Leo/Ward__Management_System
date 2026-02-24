using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Models.Domain;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
       
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;       
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View(); // Initialize a new user
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(user);

                user.EmployeeNumber = await GenerateNextEmployeeNumberAsync();

                bool addResults = await _userRepository.AddUserAsync(user);

                if (addResults)
                {
                    TempData["msg"] = "Employee information has been successfully added.";
                }
                else
                {
                    TempData["msg"] = "Failed to add employee information. Please check details and try again or contact the administrator.";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!!" + ex.Message;
            }

            return RedirectToAction(nameof(AddUser));
        }

        private async Task<string> GenerateNextEmployeeNumberAsync()
        {
            var lastEmployeeNumber = await _userRepository.GetLastEmployeeNumberAsync();

            int lastNumber;
            if (!string.IsNullOrEmpty(lastEmployeeNumber))
            {
                // Extract the number part from the EmployeeNumber
                var lastNumberString = lastEmployeeNumber[3..]; // Skip 'Emp'
                if (int.TryParse(lastNumberString, out lastNumber))
                {
                    lastNumber++;
                }
            }
            else
            {
                lastNumber = 1; // Start from 1 if no employees exist
            }

            // Format the new employee number
            return $"Emp{lastNumber:00}";
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            // Retrieve patient from database or other data source based on id
            var result = await _userRepository.GetUserByIdAsync(id);

            if (result == null)
            {
                return NotFound(); 
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            try
            {
                // If the model state is invalid, return the view with the user data
                if (!ModelState.IsValid)
                    return View(user);    

                // Update the user details
                bool update = await _userRepository.UpdateUserAsync(user);

                if (update)
                    TempData["msg"] = "Employee information has been successfully updated.";
                else
                    TempData["msg"] = "Update failed. Please verify the information and try again or contact the administrator.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong! " + ex.Message;
            }

            return RedirectToAction(nameof(DisplayAllUser));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllUser(string? search)
        {
            var results = await _userRepository.GetAllUsersAsync();

            // If search term is provided, filter; otherwise, return all
            if (!string.IsNullOrEmpty(search))
            {
                results = results.Where(p => p.EmployeeNumber.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(results);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleteResult = await _userRepository.DeletUserAsync(id);
            if (deleteResult)
            {
                SetTempDataMessage("Employee information deleted successfully.");
            }
            else
            {
                SetTempDataMessage("Employee information could not be deleted.");
            }
            return RedirectToAction(nameof(DisplayAllUser));
        }

        //this is for Temp messages
        private void SetTempDataMessage(string message)
        {
            TempData["msg"] = message;
        }
    }
}
