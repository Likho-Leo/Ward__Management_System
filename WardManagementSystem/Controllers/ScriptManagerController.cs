using Microsoft.AspNetCore.Mvc;

namespace WardManagementSystem.Controllers
{
    public class ScriptManagerController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
