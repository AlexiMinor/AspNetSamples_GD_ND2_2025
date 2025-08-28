using System.ComponentModel.DataAnnotations;

namespace AspNetSamples.UI.Validators;

public class SomeTextAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string text && !string.IsNullOrWhiteSpace(text))
        {
            // Custom validation logic for "SomeText"
            if (text.Length < 5)
            {
                return new ValidationResult("SomeText must be at least 5 characters long.");
            }
        }

        return ValidationResult.Success;
    }
}