using System.ComponentModel.DataAnnotations;
using StaffManagementSystem.Attributes;

namespace StaffManagement.Dtos.Employee
{
    public class CreateEmployeeDto
    {
        [Display(Name = "First name")]
        [StringLength(20, ErrorMessage ="First name can NOT more than 20 characters")]
        [Required(ErrorMessage ="First name can not be empty")]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        [StringLength(20, ErrorMessage = "Middle name can NOT more than 20 characters")]
        public string? MiddleName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(20, ErrorMessage = "Last name can NOT more than 20 characters")]
        [Required(ErrorMessage = "Last name can not be empty")]
        public string LastName { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [PhoneNumber]
        [Required(ErrorMessage = "Phone number can not be empty")]
        public string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "First name can NOT more than 100 characters")]
        [EmailAddress(ErrorMessage = "Email is INVALID format")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "First name can NOT more than 100 characters")]
        [Required(ErrorMessage = "Nationality can not be empty")]
        public string Nationality { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DateOfBirth]
        [Required(ErrorMessage = "Date of birth can not be empty")]
        public DateOnly DateOfBirth { get; set; }

        [StringLength(300, ErrorMessage = "First name can NOT more than 300 characters")]
        [Required(ErrorMessage = "Address can not be empty")]
        public string Address { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department can not be empty")]
        public int DepartmentId { get; set; }

        [Display(Name = "Position")]
        [Required(ErrorMessage = "Position can not be empty")]
        public int PositionId { get; set; }

    }
}
