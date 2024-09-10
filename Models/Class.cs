namespace ManagementSchool.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public decimal AnnualFees { get; set; }

        // Navigation property
        public ICollection<Teacher> Teachers { get; set; }
    }
}
