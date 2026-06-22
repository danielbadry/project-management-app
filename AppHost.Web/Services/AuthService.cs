namespace AppHost.Web.Services;

using AppHost.Web.Authentication;
using Shared.Dtos.Login;
using Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

public class AuthService(
    HttpClient http,
    TokenService tokenService,
    AuthenticationStateProvider authProvider,
    ILogger<AuthService> logger)
{
    private readonly HttpClient _http = http;
    private readonly TokenService _tokenService = tokenService;
    private readonly CustomAuthenticationStateProvider _authProvider =
            (CustomAuthenticationStateProvider)authProvider;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<bool> Login(
        string username,
        string password)
    {
        try
        {
            _logger.LogInformation("Attempting login for user {Username}", username);

            var response =
                await _http.PostAsJsonAsync(
                    "api/auth",
                    new
                    {
                        Username = username,
                        Password = password
                    });

            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.LogInformation(
                "Login response status code: {StatusCode}, body: {Body}",
                response.StatusCode,
                responseBody);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Login failed with status code {StatusCode}",
                    response.StatusCode);
                return false;
            }

            var result =
                await response.Content
                    .ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();

            if (result is null || result.Response is null)
            {
                _logger.LogWarning("Login response body could not be parsed as LoginResponseDto.");
                return false;
            }

            await _tokenService.SetTokenAsync(
                result.Response.Token);

            _authProvider.NotifyUserAuthenticated(
                result.Response.Token);

            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for login");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected login error");
            return false;
        }
    }

    public async Task Logout()
    {
        await _tokenService.RemoveTokenAsync();

        _authProvider.NotifyUserLoggedOut();
    }
}