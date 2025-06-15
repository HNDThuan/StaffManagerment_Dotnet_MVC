using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class AttendanceToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int ShiftId { get; set; }
        public Shift Shift { get; set; } = default!;

        /// <summary>
        ///  UTC time when the token was generated.
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///  UTC time when the token expires.  Default: +30 minutes.
        /// </summary>
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(30);

        public bool IsActive => DateTime.UtcNow <= ExpiresAt;
    }
}
