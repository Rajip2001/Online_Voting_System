using Microsoft.AspNetCore.Mvc;
using OnlineVoting.Models;
using OnlineVoting.Data;

namespace OnlineVoting.Controllers
{
    public class ElectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElectionController(ApplicationDbContext context)
        {
            _context = context;
        }

       [HttpGet("Election/ViewCandidates/{electionId}")]
        public IActionResult ViewCandidates(int electionId)
        {
            var candidates = _context.Candidates
                .Where(c => c.ElectionId == electionId)
                .ToList();

            return View(candidates);
        }

        [HttpGet]
        public IActionResult ViewVotes(int electionId)
        {
            var votes = _context.Votes
                .Where(v => v.ElectionId == electionId)
                .ToList();

            return View(votes);
        }

        [HttpGet]
        public IActionResult ElectionDetails(int electionId)
        {
            var election = _context.Elections
                .FirstOrDefault(e => e.ElectionId == electionId);

            return View(election);
        }

        [HttpGet]
        public IActionResult CheckElectionStatus(int electionId)
        {
            var election = _context.Elections
                .FirstOrDefault(e => e.ElectionId == electionId);

            if (election == null)
                return Content("Election not found.");

            return Content($"Election Status: {election.Status}");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ScheduleElection()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ScheduleElection(string Name, string StartDate, string EndDate)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(StartDate) || string.IsNullOrWhiteSpace(EndDate))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View();
            }

            if (!DateTime.TryParse(StartDate, out DateTime startDate) ||
                !DateTime.TryParse(EndDate, out DateTime endDate))
            {
                ModelState.AddModelError("", "Invalid date format.");
                return View();
            }

            if (endDate <= startDate)
            {
                ModelState.AddModelError("", "End date must be after the start date.");
                return View();
            }

            try
            {
                Election election = new Election
                {
                    Name = Name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Status = "Scheduled"
                };

                _context.Elections.Add(election);
                _context.SaveChanges();

                TempData["Message"] = "Election scheduled successfully.";
                return RedirectToAction("Index","Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error scheduling election: " + ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult UpdateElectionDetails(int electionId)
        {
            var election = _context.Elections
                .FirstOrDefault(e => e.ElectionId == electionId);

            return View(election);
        }

        [HttpPost]
        public IActionResult UpdateElectionDetails(int electionId, string name, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                return BadRequest("Invalid election data.");
            }

            var election = _context.Elections
                .FirstOrDefault(e => e.ElectionId == electionId);

            if (election == null)
                return NotFound();

            election.Name = name;
            election.StartDate = startDate;
            election.EndDate = endDate;
            election.Status = "Scheduled";

            _context.SaveChanges();

            return RedirectToAction("ElectionDetails", new { electionId = electionId });
        }

        public IActionResult GetAllElections()
        {
            var elections = _context.Elections.ToList();
            return View(elections);
        }

        public IActionResult SelectElection()
        {
            var elections = _context.Elections.ToList();
            return View(elections);
        }

        [HttpGet]
        public IActionResult DeleteElection(int id)
        {
            var election = _context.Elections.FirstOrDefault(e => e.ElectionId == id);

            if (election == null)
                return NotFound();

            return View(election);
        }
        
        [HttpPost, ActionName("DeleteElection")]
        public IActionResult DeleteElectionConfirmed(int ElectionId)
        {
            var election = _context.Elections
                .FirstOrDefault(e => e.ElectionId == ElectionId);

            if (election != null)
            {
                // Step 1: Remove related candidates
                var candidates = _context.Candidates
                    .Where(c => c.ElectionId == ElectionId)
                    .ToList();

                _context.Candidates.RemoveRange(candidates);

                // Step 2: Remove election
                _context.Elections.Remove(election);

                _context.SaveChanges();
            }

            return RedirectToAction("GetAllElections");
        }
    }
}