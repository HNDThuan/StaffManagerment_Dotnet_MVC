using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace StaffManagement.Models
{
    public class Position : UserActivity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Position Code")]
        [Required]
        public string PositionCode { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        //Set relationships*******

        //One from a Position to many Employees, who are related this Position
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
