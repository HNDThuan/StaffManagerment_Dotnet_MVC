using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Dtos.LeaveType
{
    public class CreateLeaveTypeDto
    {
        [StringLength(20, ErrorMessage = "Code can NOT be over 20 characters")]
        [Required(ErrorMessage = "Code can NOT be null")]
        public string Code { get; set; }

        [StringLength(50, ErrorMessage = "Name can NOT be over 50 characters")]
        [Required(ErrorMessage = "Name can NOT be null")]
        public string Name { get; set; }
    }
}
