using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
namespace AppHost.Web.Authentication;

public class CustomAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private readonly TokenService _tokenService;

    public CustomAuthenticationStateProvider(
        TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        var token = await _tokenService.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()));
        }

        var claims = ParseClaimsFromJwt(token);

        var identity =
            new ClaimsIdentity(claims, "jwt");

        var user =
            new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public void NotifyUserAuthenticated(
        string token)
    {
        var claims = ParseClaimsFromJwt(token);

        var identity = new ClaimsIdentity(
            claims,
            "jwt");

        var user =
            new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(user)));
    }

    public void NotifyUserLoggedOut()
    {
        var anonymous =
            new ClaimsPrincipal(
                new ClaimsIdentity());

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(
                    anonymous)));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs?.Select(kvp =>
            new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty))
            ?? Enumerable.Empty<Claim>();
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }
}