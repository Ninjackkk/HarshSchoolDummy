using ManagementSchool.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Student/Index
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.Include(s => s.Class).ToListAsync();
            return View(students);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return View(student);
        }
    }
}
