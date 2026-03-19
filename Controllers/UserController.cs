using Microsoft.AspNetCore.Mvc;
using OnlineVoting.Models;
using OnlineVoting.Data;


namespace OnlineVoting.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult InsertUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Age check (>= 18)
                    if (Convert.ToInt32((DateTime.UtcNow - user.DOB).TotalDays / 365) < 18)
                    {
                        ModelState.AddModelError(string.Empty, "User's age must be >= 18");
                        return View();
                    }

                    // Set admin status based on email or other rule
                    if (user.Email.EndsWith("@admin.com")) // example rule
                    {
                        user.IsAdmin = true;
                    }
                    else
                    {
                        user.IsAdmin = false;
                    }

                    // Add user to database
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    TempData["Message"] = "User successfully registered.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                TempData["Error"] = "Login unsuccessful";
                return View();
            }

            // Store user info in session
            HttpContext.Session.SetInt32("UserId", user.UserId);

            // Admin check
            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }

            // Voter: get current active election
            // Using server local time (DateTime.Now) assuming DB stores local time
            var currentElection = _context.Elections
                .FirstOrDefault(e => e.StartDate <= DateTime.Now && e.EndDate >= DateTime.Now);

            if (currentElection != null)
            {
                HttpContext.Session.SetInt32("ElectionId", currentElection.ElectionId);
                return RedirectToAction("SelectElection", "Election");
            }

            // If no current election, check if there are any elections at all
            bool hasAnyElection = _context.Elections.Any();
            if (hasAnyElection)
            {
                // Elections exist but none active right now
                TempData["Message"] = "No election is currently active. Please check back later.";
                return View("NoElection");
            }
            else
            {
                // No elections exist at all
                TempData["Message"] = "There are no elections available at the moment.";
                return View("NoElection");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return View();
        }

        [HttpGet]
        public IActionResult UpdateUser(int? userId)
        {
            if (!userId.HasValue)
                return View(new User
                {
                    Name = "",
                    Email = "",
                    Password = "",
                    CITNo = "",
                    Role = ""
                });

            var user = _context.Users.Find(userId.Value);

            if (user == null)
            {
                ViewData["Message"] = "No user found with the specified ID.";
                return View(new User
                {
                    Name = "",
                    Email = "",
                    Password = "",
                    CITNo = "",
                    Role = ""
                });
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Users.Update(user);
                    _context.SaveChanges();

                    TempData["Message"] = "User details updated successfully.";
                    return RedirectToAction("UserList");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View(user);
        }

        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                ViewData["Message"] = "No user found with the specified ID.";
                return View(null);
            }

            return View(user);
        }

        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["Message"] = "User successfully deleted.";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return RedirectToAction("UserList");
        }

        [HttpGet]
        public IActionResult CheckIfAdmin(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            bool isAdmin = user != null && user.IsAdmin;
            return Content(isAdmin ? "User is an Admin." : "User is not an Admin.");
        }
    }
}