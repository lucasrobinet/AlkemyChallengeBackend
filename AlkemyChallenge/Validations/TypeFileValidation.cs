using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.Validations
{
    public class TypeFileValidation: ValidationAttribute
    {
        private readonly string[] validType;

        public TypeFileValidation(string[] validType)
        {
            this.validType = validType;
        }

        public TypeFileValidation(GroupFileType groupFileType)
        {
            if (groupFileType == GroupFileType.Image)
                validType = new string[] { "image/jpeg", "image/png" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return ValidationResult.Success;

            if (!validType.Contains(formFile.ContentType))
                return new ValidationResult($"The file type must be {string.Join(", ", validType)}");

            return ValidationResult.Success;
        }
    }
}
