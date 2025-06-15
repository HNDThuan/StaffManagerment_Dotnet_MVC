using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class SystemCode : UserActivity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public ICollection<SystemCodeDetail> SystemCodeDetails { get; set; } = new List<SystemCodeDetail>();
    }
}
