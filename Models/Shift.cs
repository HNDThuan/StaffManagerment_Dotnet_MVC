using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class Shift
    {
        public int ShiftId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;    

        [Required]
        public TimeOnly StartTime { get; set; }

        public int GraceMinutes { get; set; } = 10; // minute in Unit

        [Required]
        public TimeOnly CutoffTime { get; set; }         

        public TimeOnly EndTime { get; set; }           

        public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public ICollection<AttendanceToken> AttendanceTokens { get; set; } = new List<AttendanceToken>();
    }


}
