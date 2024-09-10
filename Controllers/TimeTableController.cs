using ManagementSchool.Data;
using ManagementSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class TimeTableController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TimeTableController(SchoolDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private string GetCurrentUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }

        // GET: Timetable/Upload
        public async Task<IActionResult> Upload()
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

            ViewData["ClassId"] = new SelectList(await _context.Classes.Where(c => c.ClassId == teacher.ClassId).ToListAsync(), "ClassId", "ClassName");
            return View();
        }

        // POST: Timetable/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int ClassId, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_env.WebRootPath, "timetables", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var timetable = new Timetable
                {
                    ClassId = ClassId,
                    FilePath = $"/timetables/{file.FileName}"
                };

                _context.Add(timetable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Timetable/Index
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

            var timetables = await _context.Timetables
                .Where(t => t.ClassId == teacher.ClassId)
                .Include(t => t.Class)
                .ToListAsync();

            return View(timetables);
        }
    }
}
