using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Register;

public class RegisterFormDataDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(12)]
    [MaxLength(128)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MinLength(12)]
    [MaxLength(128)]
    public string ConfirmPassword { get; set; } = string.Empty;

}
