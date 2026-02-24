using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository.Nusrse;
using WardDapperMVC.Models.Domain.Nurse;

namespace WardManagementSystem.Controllers.Nurse
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo   = userRepo;
        }
        
        [HttpGet("GetEmployeeDetailsByNo")]
        public async Task<IActionResult> GetEmployeeDetailsByNo(string EmployeeNumber)
        {
            var user = await _userRepo.GetEmployeeDetailsByNoAsync(EmployeeNumber);
            if (user != null)
            {
                var jsonResponse = new
                {
                    success = true,
                    data = new { userId = user.UserId, firstName = user.FirstName, lastName = user.LastName }
                };
                Console.WriteLine($"JSON Response: {Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse)}"); // Log JSON response
                return Json(jsonResponse);
            }
            else
            {
                return Json(new { success = false, message = "Employee not found." });
            }


        }
    }
}
