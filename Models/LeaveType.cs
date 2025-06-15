using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class LeaveType : UserActivity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();
    }
}
