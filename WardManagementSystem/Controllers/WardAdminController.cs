using Microsoft.AspNetCore.Mvc;
using WardDapperMVC.Repository;

namespace WardManagementSystem.Controllers
{
    public class WardAdminController : Controller
    {
        private readonly IPatientRepository _patientRepository;

        public WardAdminController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IActionResult>Home()
        {
            // Check for new referrals
            var newReferralCount = await _patientRepository.GetNewReferralCountAsync(); // Implement this method in your repository

            // Store count in ViewData
            ViewData["NewReferralCount"] = newReferralCount;
            return View();
        }
    }
}
