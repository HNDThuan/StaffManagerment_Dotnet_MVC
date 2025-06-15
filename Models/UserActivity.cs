using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    //Used to track changes on any record
    public class UserActivity
    {
        public string? CreatedById { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string? ModifiedById { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }
    }
    public class ApprovalActivity : UserActivity
    {
        public string? ApprovalById { get; set; }

        [Required]
        public DateTime ApprovalOn { get; set; }

      
    }
}
