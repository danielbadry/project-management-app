using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
namespace AppHost.Web.Components.Pages;

public partial class Login
{
    public Dtos.Login LoginDto { set; get; } = new();

    private string? LoginErrorMessage { get; set; }

    [Inject]
    private AuthService AuthService { get; set; } = null!;

    [Inject]
    private TokenService TokenService { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        var token = await TokenService.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            Navigation.NavigateTo("/dashboard");
        }
    }

    public async Task HandleLogin()
    {
        LoginErrorMessage = null;

        var result =
                    await AuthService.Login(
                        LoginDto.Username,
                        LoginDto.Password);

        if (result.Succeeded)
        {
            Navigation.NavigateTo("/dashboard");
            return;
        }

        LoginErrorMessage = result.ErrorMessage;
    }

}
