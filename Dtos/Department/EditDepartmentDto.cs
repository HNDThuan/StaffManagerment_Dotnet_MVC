using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Dtos.Department
{
    public class EditDepartmentDto : CreateDepartmentDto
    {
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Department code is NOT less than 4 or larger than 20 characters")]
        [Required(ErrorMessage = "Department code is NOT empty")]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage = "Id is NOT empty")]
        public int Id { get; set; }
    }
}
