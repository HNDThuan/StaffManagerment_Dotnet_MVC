using System.ComponentModel.DataAnnotations;

namespace StaffManagementSystem.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        public long MaxFileSize { get; }

        public MaxFileSizeAttribute(long maxFileSize)
        {
            MaxFileSize = maxFileSize;
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

            foreach (var file in files)
            {
                if (file.Length > MaxFileSize)
                {
                    return new ValidationResult($"The file's size '{file.FileName}' is too big. The maximum file's size is {MaxFileSize / 1024 / 1024} MB.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
