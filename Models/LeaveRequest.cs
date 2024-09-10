using System.ComponentModel.DataAnnotations;

namespace ManagementSchool.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveApplicationId { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
    }
}
