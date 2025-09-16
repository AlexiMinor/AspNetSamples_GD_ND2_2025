using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.Models;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    [Remote(action: "IsEmailInUse", controller: "Account")] //js call to check if email is in use
    public string Email { get; set; }
    public string? UserName { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}