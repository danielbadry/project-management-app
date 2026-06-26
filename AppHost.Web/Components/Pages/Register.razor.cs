namespace AppHost.Web.Components.Pages;

using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.Register;

public partial class Register
{
    public RegisterFormDataDto RegisterDto { set; get; } = new();

    private string? RegisterErrorMessage { get; set; }

    [Inject]
    private RegisterService RegisterService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    public async Task HandleRegister()
    {
        RegisterErrorMessage = null;

        var result = await RegisterService.RegisterUser(RegisterDto);

        if (result.Succeeded)
        {
            Navigation.NavigateTo("/dashboard");
            return;
        }

        RegisterErrorMessage = result.ErrorMessage;
    }

}
