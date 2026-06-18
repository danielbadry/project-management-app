using Dtos = AppHost.Web.Dtos;
using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
namespace AppHost.Web.Components.Pages;

public partial class Login
{
    public Dtos.Login LoginDto { set; get; } = new();

    [Inject]
    private AuthService AuthService { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    public async Task HandleLogin()
    {
        var success =
                    await AuthService.Login(
                        LoginDto.Username,
                        LoginDto.Password);

        if (success)
        {
            Navigation.NavigateTo("/");
        }
    }

}
