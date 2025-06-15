using System.ComponentModel.DataAnnotations;

namespace StaffManagementSystem.Attributes
{
    public class MaxFilesAttribute : ValidationAttribute
    {
        public int MaxFiles { get; set; }

        public MaxFilesAttribute(int maxFiles)
        {
            MaxFiles = maxFiles;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var files = value as IFormFile[];

            if (files == null)
            {
                return new ValidationResult("File is invalid.");
            }

            if (files.Length > MaxFiles)
            {
                return new ValidationResult($"Can NOT upload more than {MaxFiles} files.");
            }

            return ValidationResult.Success;
        }
    }
}
