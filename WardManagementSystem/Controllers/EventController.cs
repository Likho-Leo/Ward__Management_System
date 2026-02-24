using WardDapperMVC.Model.Domain;
using WardDapperMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Schedule.Controllers
{
    public class EventController : Controller
    {
        private readonly IVisitScheduleRepository _repository;

        public EventController(IVisitScheduleRepository repository)
        {
            _repository = repository;
        }

        // GET: /Event/
        public async Task<IActionResult> Index()
        {
            var visits = await _repository.GetAllAsync();

            foreach (var visit in visits)
            {
                visit.DoctorFullName = await _repository.GetDoctorFullNameAsync(visit.DoctorID);
                // Optionally fetch PatientFullName if needed
            }

            return View(visits);
        }


        public async Task<IActionResult> GetEvents()
        {
            var visits = await _repository.GetAllAsync();
            var calendarEvents = visits.Select(e => new
            {
                id = e.ScheduleID,
                title = e.VisitType,
                start = e.Date.ToString("yyyy-MM-dd"),
                next = e.FollowUpAppointmentDate, // Adjust as needed for your duration
                description = e.VisitType // Optional: if you want to display a description
            });
            return Json(calendarEvents);
        }


        // GET: /Event/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var visit = await _repository.GetByIdAsync(id);
                
                if (visit == null)
                {
                    return NotFound();
                }
                return View(visit);

            }
            catch (Exception ex)
            {
                // Log exception (consider using ILogger for logging)
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: /Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Event/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FollowUpAppointmentDate")] VisitSchedule visit)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(visit);
                return RedirectToAction(nameof(Index));
            }
            return View(visit);
        }

        // GET: /Event/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var visit = await _repository.GetByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            return View(visit);
        }

        // POST: /Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitID,Date,VisitType,DoctorID,PatientID,InActive")] VisitSchedule visit)
        {
            if (id != visit.ScheduleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(visit);
                return RedirectToAction(nameof(Index));
            }
            return View(visit);
        }

        // GET: /Event/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var visit = await _repository.GetByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            return View(visit);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, VisitSchedule visit)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
