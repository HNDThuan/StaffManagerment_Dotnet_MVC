using System.ComponentModel.DataAnnotations;

namespace StaffManagementSystem.Attributes
{
    public class ImageFileAttribute : ValidationAttribute
    {
        public ImageFileAttribute() : base("ImageFile is NOT valid format")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var files = value as IFormFile[];

            if (files.Length < 1)
            {
                return new ValidationResult("ImageFile is NOT valid.");
            }

            //Check ext(Just accept ext: .jpg, .jpeg, .png, .gif, .bmp, .webp)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
                {
                    return new ValidationResult("ImageFile just accept the file with these extension .jpg, .jpeg, .png, .gif, .bmp, .webp.");

                }
            }
            return ValidationResult.Success;
        }
    }
}
