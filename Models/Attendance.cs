using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace StaffManagement.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        [Required]
        public int ShiftId { get; set; }
        public Shift Shift { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [DataType(DataType.Time)]
        public TimeOnly? CheckInTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly? CheckOutTime { get; set; }

        // Phút đi trễ (0 nếu đúng giờ, -1 = quá Cutoff => absent)
        public int MinutesLate { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal Penalty { get; set; }

        public bool IsLate => MinutesLate > 0;
        public bool IsAbsent => MinutesLate == -1;
        public bool IsCompleted => CheckInTime.HasValue && CheckOutTime.HasValue;

    }
}
