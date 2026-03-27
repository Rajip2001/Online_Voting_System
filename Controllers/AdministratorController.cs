using Microsoft.AspNetCore.Mvc;
using OnlineVoting.Data;
using OnlineVoting.Models;
using OnlineVoting.Models.ViewModels;

namespace OnlineVoting.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Optional: fetch some dashboard info
            var elections = _context.Elections.ToList();
            var candidates = _context.Candidates.ToList();
            var voters = _context.Users.Where(u => u.Role == "Voter").ToList();

            // Pass data to the view if needed
            ViewBag.Elections = elections;
            ViewBag.Candidates = candidates;
            ViewBag.Voters = voters;

            return View(); // Looks for Views/Admin/Index.cshtml
        }
        public IActionResult AcceptCandidate(int candidateId)
        {
            var candidate = _context.Candidates.Find(candidateId);
            if (candidate != null)
            {
                candidate.IsAccepted = true; // Optional: mark as accepted
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteCandidate(int candidateId)
        {
            var candidate = _context.Candidates.Find(candidateId);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult GetVoterList()
        {
            var voters = _context.Users
                .Where(u => u.Role == "Voter")
                .ToList();

            return View(voters);
        }

        public bool IsAdmin(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user != null && user.IsAdmin;
        }

        public IActionResult StartElection(int electionId)
        {
            HttpContext.Session.SetInt32("ElectionId", electionId);

            var election = _context.Elections.Find(electionId);
            if (election != null)
            {
                election.Status = "Started";
                _context.SaveChanges();
                Console.WriteLine($"Election with ID {electionId} has started.");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EndElection(int electionId)
        {
            if (HttpContext.Session.Keys.Contains("ElectionId"))
                HttpContext.Session.Remove("ElectionId");

            var election = _context.Elections.Find(electionId);
            if (election != null)
            {
                election.Status = "Ended";
                _context.SaveChanges();
                Console.WriteLine($"Election with ID {electionId} has ended.");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Admin/ElectionResult/{electionId}")]
        public IActionResult ElectionResult(int electionId)
        {
            var election = _context.Elections
                .FirstOrDefault(e => e.ElectionId == electionId);

            if (election == null)
                return NotFound("Election not found.");

            var candidates = _context.Candidates
                .Where(c => c.ElectionId == electionId)
                .ToList();

            var votes = _context.Votes
                .Where(v => v.ElectionId == electionId)
                .ToList();

            int totalVotes = votes.Count;

            // ✅ Assign VoteCount properly
            foreach (var candidate in candidates)
            {
                candidate.VoteCount = votes.Count(v => v.CandidateId == candidate.CandidateId);
            }

            // ✅ Winner based on calculated VoteCount
            var winner = candidates
                .OrderByDescending(c => c.VoteCount)
                .FirstOrDefault();

            // Election not ended → no winner
            if (DateTime.UtcNow < election.EndDate)
                winner = null;

            int totalVoters = _context.Users.Count(u => u.Role == "Voter");
            double turnoutPercentage = totalVoters > 0 ? (totalVotes / (double)totalVoters) * 100 : 0;

            var resultViewModel = new ResultViewModel
            {
                ElectionDetails = election,
                Winner = winner,
                TotalVotes = totalVotes,
                TurnoutPercentage = turnoutPercentage
            };

            return View(resultViewModel);
        }
    }
}