using ManagementSchool.Data;
using ManagementSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly SchoolDbContext _context;

        public AttendanceController(SchoolDbContext context)
        {
            _context = context;
        }

        // Helper function to get the logged-in teacher's UserId
        private string GetCurrentUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }

        // GET: Attendance/Mark
        public async Task<IActionResult> Mark()
        {
            // Get the logged-in teacher's UserId
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the teacher from the database based on UserId
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            // Fetch students in the teacher's class
            var students = await _context.Students.Where(s => s.ClassId == teacher.ClassId).ToListAsync();

            // Set the ClassId and Date in ViewBag to pass to the view
            ViewBag.ClassId = teacher.ClassId;
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");

            return View(students); // Return the students to the view
        }

        // POST: Attendance/Mark
        [HttpPost]
        public async Task<IActionResult> Mark(List<int> presentStudentIds, int classId, string date)
        {
            DateTime attendanceDate = DateTime.Parse(date);

            // Fetch students in the class
            var students = await _context.Students.Where(s => s.ClassId == classId).ToListAsync();

            // Record attendance for each student in the class
            foreach (var student in students)
            {
                var attendance = new Attendance
                {
                    StudentId = student.StudentId,
                    Date = attendanceDate,
                    IsPresent = presentStudentIds.Contains(student.StudentId) // Mark as present if in list
                };
                _context.Add(attendance);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to attendance listing
        }

        // GET: Attendance/Index
        public async Task<IActionResult> Index()
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

            var attendanceList = await _context.Attendances
                .Include(a => a.Student)
                .Where(a => a.Student.ClassId == teacher.ClassId) // Filter by teacher's class
                .ToListAsync();

            return View(attendanceList); // Show attendance records for the teacher's class
        }
    }
}
