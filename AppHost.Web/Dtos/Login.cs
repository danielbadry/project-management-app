using System.ComponentModel.DataAnnotations;

namespace AppHost.Web.Dtos;

public class Login
{
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3)]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    [MinLength(3)]
    public string Password { get; set; } = "";

}