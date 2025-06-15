using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Dtos.Department
{
    public class CreateDepartmentDto
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Location { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
