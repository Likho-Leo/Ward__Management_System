using Microsoft.AspNetCore.Mvc;

namespace WardManagementSystem.Controllers
{
    public class NurseController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
