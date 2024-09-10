using ManagementSchool.Data;
using ManagementSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SchoolDbContext _context;

        public TeacherController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Teachers/Create
        public async Task<IActionResult> Create()
        {
            // Fetch classes and subjects from the database
            ViewData["Classes"] = new SelectList(await _context.Classes.ToListAsync(), "ClassId", "ClassName");
            ViewData["Subjects"] = new SelectList(await _context.Subjects.ToListAsync(), "SubjectId", "SubjectName");
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Qualification,Email,HireDate,UserId,Password,MonthlySalary,ClassId,SubjectId")] Teacher teacher)
        {
            
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login","Account");
            
            // Re-populate dropdown lists in case of validation errors
            ViewData["Classes"] = new SelectList(await _context.Classes.ToListAsync(), "ClassId", "ClassName", teacher.ClassId);
            ViewData["Subjects"] = new SelectList(await _context.Subjects.ToListAsync(), "SubjectId", "SubjectName", teacher.SubjectId);

            return View(teacher);
        }

        // List of Teachers
        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers.Include(t => t.Class).Include(t => t.Subject).ToListAsync();
            return View(teachers);
        }
    }
}
