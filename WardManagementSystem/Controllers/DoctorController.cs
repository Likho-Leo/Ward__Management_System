using Microsoft.AspNetCore.Mvc;

namespace WardManagementSystem.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
