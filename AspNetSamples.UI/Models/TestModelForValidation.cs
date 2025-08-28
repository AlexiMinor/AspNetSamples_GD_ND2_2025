using System.ComponentModel.DataAnnotations;

namespace AspNetSamples.UI.Models;

public class TestModelForValidation
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format")] //non RFC validation
    public string Email { get; set; }

    //[Required]
    [RegularExpression("")]
    public string Password { get; set; }

    [Compare(nameof(Password))]
    //[Compare("Password")]
    public string PasswordConfirmation { get; set; }

    public string Name { get; set; }

    [Range(1, 100, ErrorMessage = "Value must be between 1 and 100")]
    public int SomeValue { get; set; }

    [Required]
    [Phone] // This attribute validates phone numbers, but the validation is not RFC compliant
    public string Phone { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    public string Url { get; set; }

    [CreditCard(ErrorMessage = "Invalid credit card number format")]
    public string CreditCard { get; set; }
}