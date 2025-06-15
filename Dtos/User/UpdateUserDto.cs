using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Dtos.User
{
    public class UpdateUserDto
    {

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "UserName")]
        public string? UserName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; } = default!;

        [Display(Name = "Employee")]
        public int? EmployeeId { get; set; }

        [Display(Name = "User Role")]
        [MinLength(1, ErrorMessage = "The {1} is required at least {1} role(s)")]
        public List<string> RoleNames { get; set; } = [];
    }
}
