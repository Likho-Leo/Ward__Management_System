using Microsoft.AspNetCore.Mvc;

namespace WardManagementSystem.Controllers
{
    public class NursingSisterController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
