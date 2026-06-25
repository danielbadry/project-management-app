using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Register;

public class RegisterFormDataDto
{
    [Required]
    [MinLength(5)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(3)]
    public string Username { get; set; }

    [Required]
    [MinLength(5)]
    public string Password { get; set; }

    [Required]
    [MinLength(5)]
    public string ConfirmPassword { get; set; }

}