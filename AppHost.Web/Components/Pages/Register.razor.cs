namespace AppHost.Web.Components.Pages;

using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.Register;

public partial class Register
{
    public RegisterFormDataDto RegisterDto { set; get; } = new();

    [Inject]
    private RegisterService RegisterService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    public async Task HandleRegister()
    {
        var success = await RegisterService.RegisterUser(RegisterDto);

        if (success)
        {
            Navigation.NavigateTo("/");
        }
    }

}