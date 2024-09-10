namespace ManagementSchool.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }

        // New Properties
        public string UserId { get; set; }
        public string Password { get; set; }
        public decimal MonthlySalary { get; set; }

        // Foreign Key Relationships
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}

