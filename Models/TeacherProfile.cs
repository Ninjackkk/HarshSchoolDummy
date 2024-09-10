using System.ComponentModel.DataAnnotations;

namespace ManagementSchool.Models
{
    public class TeacherProfile
    {
        [Key]
        public string TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Qualification { get; set; }
        public decimal MonthlySalary { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}
