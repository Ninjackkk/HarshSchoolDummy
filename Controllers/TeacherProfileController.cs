using ManagementSchool.Data;
using ManagementSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class TeacherProfileController : Controller
    {
        

        private readonly SchoolDbContext _context;

        public TeacherProfileController(SchoolDbContext context)
        {
            _context = context;
        }

        private string GetCurrentUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }

        // GET: TeacherProfile/Details
        public async Task<IActionResult> Edit()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            var profile = new TeacherProfile
            {
                TeacherId = userId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                Qualification = teacher.Qualification,
                MonthlySalary = teacher.MonthlySalary
            };

            return View(profile);
        }
    }
}
