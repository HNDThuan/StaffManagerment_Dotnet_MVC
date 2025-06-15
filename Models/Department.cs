using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class Department : UserActivity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Department Code")]
        [Required]
        public string DepartmentCode { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Location { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        //Set relationships*******
        //One from a Department to many Employees, who are in this Department
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

      
    }
}
