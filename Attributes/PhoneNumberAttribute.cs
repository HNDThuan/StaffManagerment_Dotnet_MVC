using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StaffManagementSystem.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {

        int MaxLenght { get; set; } = 10;
        int MinLenght { get; set; } = 10;
        public PhoneNumberAttribute() : base("Phone number is INVALID format.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string phoneNumber = value.ToString();

            if (phoneNumber.Length < MinLenght || phoneNumber.Length > MaxLenght)
            {
               return new ValidationResult($"Phone number can NOT less than {MinLenght}, or greater than {MaxLenght}");
            }

            string pattern = @"^(03|05|07|08|09|01[2|6|8|9])\d{8}$";
            Regex regex = new Regex(pattern);

            if (regex.IsMatch(phoneNumber))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
