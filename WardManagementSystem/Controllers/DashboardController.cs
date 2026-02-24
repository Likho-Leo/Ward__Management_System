using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public DashboardController(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _statisticsRepository.GetStatistics(); // Correctly using await
            return View(model);
        }
    
    }
}
