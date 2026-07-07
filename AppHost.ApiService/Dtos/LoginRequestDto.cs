using System.ComponentModel.DataAnnotations;

namespace AppHost.ApiService.Dtos;

public class LoginRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string Password { get; set; } = string.Empty;
}
