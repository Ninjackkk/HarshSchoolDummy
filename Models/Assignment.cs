namespace ManagementSchool.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string FilePath { get; set; } // for uploaded assignment file

        // Foreign Key Relationships
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }



    public class AssignmentResponse
    {
        public int AssignmentResponseId { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public string ResponseFilePath { get; set; } // for student assignment file
        public DateTime SubmittedOn { get; set; }
    }
}
