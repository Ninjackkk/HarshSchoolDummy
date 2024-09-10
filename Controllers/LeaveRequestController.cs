using ManagementSchool.Data;
using ManagementSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSchool.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly SchoolDbContext _context;

        public LeaveRequestController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Leave/Create
        public IActionResult Create()
        {
            return View();
        }

        private string GetCurrentUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }
        // POST: Leave/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FromDate,ToDate,Reason")] LeaveRequest leave)
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
            leave.TeacherId = teacher.TeacherId;

                _context.Add(leave);
                await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));

            //return View(leave);
        }
    }
}
