using Microsoft.AspNetCore.Mvc;
using OnlineVoting.Data;
using OnlineVoting.Models;

namespace OnlineVoting.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddCandidate(int id)
        {
            ViewBag.ElectionId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCandidate(
            string name,
            int age,
            string citno,
            string edu,
            string party,
            IFormFile logo,
            IFormFile profile,
            string manifesto,
            int electionId)
        {
            Candidate candidate = new Candidate
            {
                Name = name,
                Age = age,
                CITNo = citno,
                Party = party,
                Education = edu,
                Manifesto = manifesto,
                ElectionId = electionId,
                VoteCount = 0
            };


            if (profile != null && profile.Length > 0 && logo != null && logo.Length > 0)
            {
                try
                {
                    var profileFileName = Guid.NewGuid().ToString() + Path.GetExtension(profile.FileName);
                    var logoFileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.FileName);

                    var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (!Directory.Exists(imagesDirectory))
                    {
                        Directory.CreateDirectory(imagesDirectory);
                    }

                    var profilePath = Path.Combine(imagesDirectory, profileFileName);
                    var logoPath = Path.Combine(imagesDirectory, logoFileName);

                    using (var stream = new FileStream(profilePath, FileMode.Create))
                    {
                        await profile.CopyToAsync(stream);
                    }

                    using (var stream = new FileStream(logoPath, FileMode.Create))
                    {
                        await logo.CopyToAsync(stream);
                    }

                    candidate.ProfilePicture = "/images/" + profileFileName;
                    candidate.Logo = "/images/" + logoFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error uploading image: " + ex.Message);
                    return View();
                }
            }

            // Save candidate to database
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Candidate added successfully!";
            return RedirectToAction("Index","Admin");
        }
    }
}