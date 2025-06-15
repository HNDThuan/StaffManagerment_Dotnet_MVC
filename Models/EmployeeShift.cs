using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class EmployeeShift
    {
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        [Required]
        public int ShiftId { get; set; }
        public Shift Shift { get; set; } = default!;

        [Required]
        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}
