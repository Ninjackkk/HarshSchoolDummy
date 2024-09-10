namespace ManagementSchool.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        // Navigation property
        public ICollection<Teacher> Teachers { get; set; }
    }
}
