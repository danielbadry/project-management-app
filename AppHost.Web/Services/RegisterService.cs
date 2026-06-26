namespace AppHost.Web.Services;

using AppHost.Web.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Dtos.Register;

public class RegisterService(ApiClient apiClient,
    TokenService tokenService,
    AuthenticationStateProvider authProvider)
{
    private readonly string registerUrl = "api/register";
    private readonly ApiClient _apiClient = apiClient;

    private readonly TokenService _tokenService = tokenService;
    private readonly CustomAuthenticationStateProvider _authProvider =
            (CustomAuthenticationStateProvider)authProvider;

    public async Task<ServiceResult> RegisterUser(
        RegisterFormDataDto registerDto)
    {
        var result = await _apiClient.PostAsync<RegisterResponseDto>(
           registerUrl,
           registerDto,
           "register user",
           "Registration failed. Please check your details.");


        if (!result.Succeeded || result.Data is null)
        {
            return ServiceResult.Failure(
                result.ErrorMessage ?? "Login failed. Please try again.");
        }

        await _tokenService.SetTokenAsync(result.Data.Token);

        _authProvider.NotifyUserAuthenticated(result.Data.Token);

        return ServiceResult.Success();
    }
}
