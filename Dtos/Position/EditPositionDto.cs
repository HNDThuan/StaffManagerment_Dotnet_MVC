using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Dtos.Position
{
    public class EditPositionDto : CreatePositionDto
    {
        [StringLength(20, MinimumLength = 4, ErrorMessage ="Positiion code is NOT less than 4 or larger than 20 characters")]
        [Required(ErrorMessage = "Position code is NOT empty")]
        public string PositionCode { get; set; }

        [Required(ErrorMessage = "Id is NOT empty")]
        public int Id { get; set; }
    }
}
