using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Dtos.Position
{
    public class CreatePositionDto
    {
        [StringLength(50, ErrorMessage = "Name code can NOT be greater than 50 character")]
        [Required(ErrorMessage = "Name can NOT be null")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Description code can NOT be greater than 250 character")]
        public string? Description { get; set; }
    }
}
