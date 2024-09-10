using ManagementSchool.Data;
using ManagementSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class AccountController : Controller
    {
        private readonly SchoolDbContext _context;

        public AccountController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.UserId == model.UserId && t.Password == model.Password);

            if (teacher == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Log in the user
            // For simplicity, let's store the userId in a session
            HttpContext.Session.SetString("UserId", model.UserId);

            return RedirectToAction("Create", "Assignment"); // Redirect to the assignments page or any other page
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }
    }
}
