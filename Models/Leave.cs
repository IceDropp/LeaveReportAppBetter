using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveReportAppBetter.Models
{
    public enum LeaveStatus
    {
        Pending, Approved, Denied
    }
    public enum LeaveType
    {
        Holliday,
        Sick,
        Peronal,
        Other
    }
    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType Type { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        [ForeignKey("Employee")]
        public int FkEmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
