namespace ManagementSchool.Models
{
    public class Timetable
    {
        public int TimetableId { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public string FilePath { get; set; } // path for the uploaded timetable
    }
}
