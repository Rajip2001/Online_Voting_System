using Microsoft.AspNetCore.Mvc;
using OnlineVoting.Data;
using OnlineVoting.Models;
using OnlineVoting.Models.ViewModels;

namespace OnlineVoting.Controllers
{
    public class VoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get candidates as JSON
        [HttpGet]
        public IActionResult GetCandidates(int electionId)
        {
            var candidates = _context.Candidates
                                     .Where(c => c.ElectionId == electionId)
                                     .ToList();
            return Json(candidates);
        }
        
        [HttpGet]
        public IActionResult CastVoteForm(int electionId)
        {
            var candidates = _context.Candidates
                .Where(c => c.ElectionId == electionId)
                .ToList();

            if (candidates == null || candidates.Count == 0)
            {
                return Content("No candidates found for this election.");
            }

            ViewBag.ElectionId = electionId;

            return View(candidates);
        }
        // Show voting form
        // [HttpGet]
        // public IActionResult CastVoteForm()
        // {
        //     int? electionId = HttpContext.Session.GetInt32("ElectionId");
        //     int? userId = HttpContext.Session.GetInt32("UserId");

        //     if (!userId.HasValue)
        //     {
        //         return RedirectToAction("Login", "User");
        //     }

        //     if (!electionId.HasValue)
        //     {
        //         // No election in session: show message
        //         return View("NoElection"); // same view as above
        //     }

        //     List<Candidate> candidates = _context.Candidates
        //         .Where(c => c.ElectionId == electionId.Value)
        //         .ToList();

        //     if (candidates.Count == 0)
        //     {
        //         TempData["Message"] = "No candidates are available in this election.";
        //         return View("NoElection");
        //     }

        //     return View(candidates);
        // }

        // Cast vote
        // [HttpPost]
        // public IActionResult CreateVote(int candidateId)
        // {
        //     int? userId = HttpContext.Session.GetInt32("UserId");
        //     int? electionId = HttpContext.Session.GetInt32("ElectionId");

        //     if (!userId.HasValue || !electionId.HasValue)
        //         return RedirectToAction("Index", "Home");

        //     // Check if user already voted
        //     bool alreadyVoted = _context.Votes
        //                                 .Any(v => v.VoterId == userId.Value && v.ElectionId == electionId.Value);

        //     if (alreadyVoted)
        //     {
        //         TempData["Error"] = "You can vote only once!";
        //         return RedirectToAction("Index", "Home");
        //     }

        //     // Create vote
        //     var vote = new Vote
        //     {
        //         VoterId = userId.Value,
        //         CandidateId = candidateId,
        //         ElectionId = electionId.Value,
        //         CastDateTime = DateTime.UtcNow
        //     };
        //     _context.Votes.Add(vote);

        //     // Update candidate's vote count
        //     var candidate = _context.Candidates.FirstOrDefault(c => c.CandidateId == candidateId);
        //     if (candidate != null)
        //         candidate.VoteCount += 1;

        //     _context.SaveChanges();

        //     TempData["Message"] = "Vote cast successfully!";
        //     return RedirectToAction("CastVoteForm");
        // }
        [HttpPost]
        public IActionResult CastVote(int candidateId, int electionId)
        {
            var candidate = _context.Candidates
                .FirstOrDefault(c => c.CandidateId == candidateId);

            if (candidate != null)
            {
                candidate.VoteCount += 1;
                _context.SaveChanges();
            }

            return RedirectToAction("CastVoteForm", "Election", new { electionId });
        }
        
        // View details of a specific vote
        [HttpGet]
        public IActionResult GetVoteDetails(int id)
        {
            var vote = _context.Votes.FirstOrDefault(v => v.VoteId == id);
            if (vote == null) return NotFound();

            var election = _context.Elections.FirstOrDefault(e => e.ElectionId == vote.ElectionId);
            var candidate = _context.Candidates.FirstOrDefault(c => c.CandidateId == vote.CandidateId);
            var user = _context.Users.FirstOrDefault(u => u.UserId == vote.VoterId);

            var viewModel = new VoteDetailsViewModel
            {
                ElectionName = election?.Name,
                CandidateName = candidate?.Name,
                CandidatePhotoUrl = candidate?.ProfilePicture,
                VoterName = user?.Name,
                VoterEmail = user?.Email,
                CastDateTime = vote.CastDateTime
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CandidateVotes(int candidateId)
        {
            var votes = _context.Votes
                                .Where(v => v.CandidateId == candidateId)
                                .ToList();
            return View(votes);
        }

        [HttpGet]
        public IActionResult ElectionVotes(int electionId)
        {
            var votes = _context.Votes
                                .Where(v => v.ElectionId == electionId)
                                .ToList();
            return View(votes);
        }
        public IActionResult SelectElection()
        {
            var elections = _context.Elections.ToList(); // or filter active only

            return View(elections);
        }

        public IActionResult ShowCandidates(int electionId)
        {
            var candidates = _context.Candidates
                .Where(c => c.ElectionId == electionId)
                .ToList();

            // store election in session
            HttpContext.Session.SetInt32("ElectionId", electionId);

            return View("CastVoteForm", candidates);
        }
        
    }
}