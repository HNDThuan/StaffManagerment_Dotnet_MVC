using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class SystemCodeDetail : UserActivity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SystemCodeId { get; set; }

        [Required]
        public string Code { get; set; }

        [StringLength(500, ErrorMessage = "Description can NOT over 500 characters")]
        public string? Description { get; set; }

        public int? OrderNumber { get; set; }

        public SystemCode? SystemCode { get; set; }

        public ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();
    }
}
