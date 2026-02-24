using Microsoft.AspNetCore.Mvc;

namespace WardManagementSystem.Controllers
{
    public class StockManagerController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
