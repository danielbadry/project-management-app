using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using AppHost.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Dtos;
using Shared.Dtos.Auth;

namespace AppHost.Web.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    private readonly TokenService _tokenService;
    private readonly HttpClient _http;

    public CustomAuthenticationStateProvider(
        TokenService tokenService,
        IHttpClientFactory httpClientFactory)
    {
        _tokenService = tokenService;
        _http = httpClientFactory.CreateClient("ApiClient");
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenService.GetTokenAsync();

        return string.IsNullOrWhiteSpace(token)
            ? CreateAnonymousState()
            : await ValidateTokenAsync(token);
    }

    public async Task NotifyUserAuthenticatedAsync(string token)
    {
        var authenticationState = await ValidateTokenAsync(token);

        NotifyAuthenticationStateChanged(
            Task.FromResult(authenticationState));
    }

    public void NotifyUserLoggedOut()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(CreateAnonymousState()));
    }

    private async Task<AuthenticationState> ValidateTokenAsync(string token)
    {
        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/auth/session");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _http.SendAsync(request);

            if (response.StatusCode is HttpStatusCode.Unauthorized or
                HttpStatusCode.Forbidden)
            {
                await _tokenService.RemoveTokenAsync();
                return CreateAnonymousState();
            }

            if (!response.IsSuccessStatusCode)
            {
                return CreateAnonymousState();
            }

            var apiResponse = await response.Content
                .ReadFromJsonAsync<ApiResponse<AuthenticatedUserDto>>(JsonOptions);

            if (apiResponse?.IsSuccess != true || apiResponse.Response is null)
            {
                return CreateAnonymousState();
            }

            return CreateAuthenticatedState(apiResponse.Response);
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException)
        {
            return CreateAnonymousState();
        }
    }

    private static AuthenticationState CreateAuthenticatedState(
        AuthenticatedUserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Name),
            new("username", user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "jwt");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private static AuthenticationState CreateAnonymousState() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));
}
