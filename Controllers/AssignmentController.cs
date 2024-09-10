using ManagementSchool.Data;
using ManagementSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AssignmentController(SchoolDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private string GetCurrentUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }

        // GET: Assignment/Create
        public async Task<IActionResult> Create()
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

            ViewData["Classes"] = new SelectList(await _context.Classes.Where(c => c.ClassId == teacher.ClassId).ToListAsync(), "ClassId", "ClassName");
            ViewData["Subjects"] = new SelectList(await _context.Subjects.Where(s => s.SubjectId == teacher.SubjectId).ToListAsync(), "SubjectId", "SubjectName");
            return View();
        }

        // POST: Assignment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,AssignDate,SubmissionDate,ClassId,SubjectId")] Assignment assignment, IFormFile file)
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

            assignment.TeacherId = teacher.TeacherId;

            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_env.WebRootPath, "assignments", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                assignment.FilePath = $"/assignments/{file.FileName}";
            }

            _context.Add(assignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Assignment/Index
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

            var assignments = await _context.Assignments
                .Where(a => a.TeacherId == teacher.TeacherId)
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .ToListAsync();

            return View(assignments);
        }

        // GET: Assignment/DownloadResponse/5
        public async Task<IActionResult> DownloadResponse(int id)
        {
            var response = await _context.AssignmentResponses.FirstOrDefaultAsync(r => r.AssignmentResponseId == id);
            if (response == null)
            {
                return NotFound("Assignment response not found.");
            }

            var path = Path.Combine(_env.WebRootPath, response.ResponseFilePath.TrimStart('/'));
            if (!System.IO.File.Exists(path))
            {
                return NotFound("File not found.");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", Path.GetFileName(path));
        }
    }
}
