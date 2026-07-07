using System.ComponentModel.DataAnnotations;

namespace AppHost.Web.Dtos;

public class Login
{
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(3)]
    [MaxLength(128)]
    public string Password { get; set; } = string.Empty;

}
