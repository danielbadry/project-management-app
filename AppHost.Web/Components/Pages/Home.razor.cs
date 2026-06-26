using AppHost.Web.Services;
using Microsoft.AspNetCore.Components;
namespace AppHost.Web.Components.Pages;

public partial class Home
{
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
}
