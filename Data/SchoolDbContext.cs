using ManagementSchool.Models;
using Microsoft.EntityFrameworkCore;
using static ManagementSchool.Models.Assignment;

namespace ManagementSchool.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }
        //
        // DbSets for each model
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentResponse> AssignmentResponses { get; set; }
        public DbSet<LeaveRequest> LeaveApplications { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the primary key for LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasKey(lr => lr.LeaveApplicationId);

            // Configure relationships for Teacher
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Class)
                .WithMany(c => c.Teachers)
                .HasForeignKey(t => t.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Subject)
                .WithMany(s => s.Teachers)
                .HasForeignKey(t => t.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data for Classes
            modelBuilder.Entity<Class>().HasData(
                new Class { ClassId = 1, ClassName = "STD 5" },
                new Class { ClassId = 2, ClassName = "STD 6" },
                new Class { ClassId = 3, ClassName = "STD 10" }
            );

            // Seed data for Subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, SubjectName = "Maths" },
                new Subject { SubjectId = 2, SubjectName = "English" },
                new Subject { SubjectId = 3, SubjectName = "Science" },
                new Subject { SubjectId = 4, SubjectName = "Hindi" },
                new Subject { SubjectId = 5, SubjectName = "Marathi" },
                new Subject { SubjectId = 6, SubjectName = "Sanskrit" },
                new Subject { SubjectId = 7, SubjectName = "Geography" },
                new Subject { SubjectId = 8, SubjectName = "History" }
            );
















        }
    }
}
