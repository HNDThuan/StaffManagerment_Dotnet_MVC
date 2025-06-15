using Microsoft.AspNetCore.Identity;

namespace StaffManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? CreatedById { get; set; }
        public DateTime? CreatedOn { get; set; }
  
        public DateTime? LoginDate { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? PasswordChangedOn { get; set; }

        public ICollection<IdentityRole> Roles { get; set; } = [];

        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }

    }
}
