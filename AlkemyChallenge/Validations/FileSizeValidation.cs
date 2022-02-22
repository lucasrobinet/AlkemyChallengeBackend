using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.Validations
{
    public class FileSizeValidation: ValidationAttribute
    {
        private readonly int maxSizeMB;

        public FileSizeValidation(int MaxSizeMB)
        {
            maxSizeMB = MaxSizeMB;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return ValidationResult.Success;

            if (formFile.Length > maxSizeMB * 1024 * 1024)
                return new ValidationResult($"The max size of file cannot be more than {maxSizeMB}mb");

            return ValidationResult.Success;
        }
    }
}
