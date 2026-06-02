namespace AppHost.Web.Services;

using AppHost.Web.Authentication;
using AppHost.Web.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly TokenService _tokenService;
    private readonly CustomAuthenticationStateProvider _authProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        HttpClient http,
        TokenService tokenService,
        AuthenticationStateProvider authProvider,
        ILogger<AuthService> logger)
    {
        _http = http;
        _tokenService = tokenService;
        _logger = logger;

        _authProvider =
            (CustomAuthenticationStateProvider)authProvider;
    }

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
                    .ReadFromJsonAsync<LoginResponseDto>();

            if (result is null)
            {
                _logger.LogWarning("Login response body could not be parsed as LoginResponseDto.");
                return false;
            }

            await _tokenService.SetTokenAsync(
                result.Token);

            _authProvider.NotifyUserAuthenticated(
                result.Token);

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