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
            return CreateAnonymousState();
        }

        if (!TryParseClaimsFromJwt(token, out var claims))
        {
            await _tokenService.RemoveTokenAsync();
            return CreateAnonymousState();
        }

        var identity =
            new ClaimsIdentity(claims, "jwt");

        var user =
            new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public void NotifyUserAuthenticated(
        string token)
    {
        if (!TryParseClaimsFromJwt(token, out var claims))
        {
            NotifyUserLoggedOut();
            return;
        }

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
        NotifyAuthenticationStateChanged(
            Task.FromResult(
                CreateAnonymousState()));
    }

    private static AuthenticationState CreateAnonymousState() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private static bool TryParseClaimsFromJwt(
        string jwt,
        out IEnumerable<Claim> claims)
    {
        claims = Enumerable.Empty<Claim>();
        var segments = jwt.Split('.');

        if (segments.Length != 3 || string.IsNullOrWhiteSpace(segments[1]))
        {
            return false;
        }

        try
        {
            var jsonBytes = ParseBase64UrlWithoutPadding(segments[1]);
            var keyValuePairs =
                JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);

            if (keyValuePairs is null)
            {
                return false;
            }

            claims = keyValuePairs.Select(kvp =>
                new Claim(kvp.Key, kvp.Value.ToString()));
            return true;
        }
        catch (Exception ex) when (
            ex is FormatException or JsonException or ArgumentException)
        {
            return false;
        }
    }

    private static byte[] ParseBase64UrlWithoutPadding(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
            case 1: throw new FormatException("Invalid Base64Url value.");
        }

        return Convert.FromBase64String(base64);
    }
}
