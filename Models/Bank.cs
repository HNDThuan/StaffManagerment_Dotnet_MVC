using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class Bank: UserActivity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AccountNumber { get; set; }
    }
}
