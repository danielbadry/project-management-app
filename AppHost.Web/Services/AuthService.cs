namespace AppHost.Web.Services;

using AppHost.Web.Authentication;
using Shared.Dtos.Login;
using Microsoft.AspNetCore.Components.Authorization;

public class AuthService(
    ApiClient apiClient,
    TokenService tokenService,
    AuthenticationStateProvider authProvider)
{
    private readonly ApiClient _apiClient = apiClient;
    private readonly TokenService _tokenService = tokenService;
    private readonly CustomAuthenticationStateProvider _authProvider =
            (CustomAuthenticationStateProvider)authProvider;

    public async Task<ServiceResult> Login(
        string username,
        string password)
    {
        var result = await _apiClient.PostAsync<LoginResponseDto>(
            "api/auth",
            new
            {
                Username = username,
                Password = password
            },
            "login",
            "Login failed. Please check your username and password.");

        if (!result.Succeeded || result.Data is null)
        {
            return ServiceResult.Failure(
                result.ErrorMessage ?? "Login failed. Please try again.");
        }

        await _tokenService.SetTokenAsync(result.Data.Token);

        _authProvider.NotifyUserAuthenticated(result.Data.Token);

        return ServiceResult.Success();
    }

    public async Task Logout()
    {
        await _tokenService.RemoveTokenAsync();

        _authProvider.NotifyUserLoggedOut();
    }
}
