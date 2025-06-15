using System.ComponentModel.DataAnnotations;

namespace StaffManagementSystem.Attributes
{
    public class DateOfBirthAttribute : ValidationAttribute
    {
        public DateOfBirthAttribute() : base("Date of Birth is invalid format or it is less than 18")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; 
            }

            DateTime dateOfBirth;

            if (!DateTime.TryParse(value.ToString(), out dateOfBirth))
            {
                return new ValidationResult("Date of birth is INVALID format.");
            }

            int age = DateTime.Now.Year - dateOfBirth.Year;

            if (age < 18)
            {
                return new ValidationResult("Age must be from 18 above.");
            }

            return ValidationResult.Success;
        }
    }
}
