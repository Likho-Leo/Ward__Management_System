using Microsoft.AspNetCore.Mvc;

namespace WardManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
