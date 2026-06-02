namespace AppHost.Web.Services;

using AppHost.Web.Authentication;
using AppHost.Web.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly TokenService _tokenService;
    private readonly CustomAuthenticationStateProvider _authProvider;

    public AuthService(
        HttpClient http,
        TokenService tokenService,
        AuthenticationStateProvider authProvider)
    {
        _http = http;
        _tokenService = tokenService;

        _authProvider =
            (CustomAuthenticationStateProvider)authProvider;
    }

    public async Task<bool> Login(
        string username,
        string password)
    {
        // var response =
        //     await _http.PostAsJsonAsync(
        //         "api/auth/login",
        //         new
        //         {
        //             Username = username,
        //             Password = password
        //         });
        await Task.Delay(500); // simulate network delay

        var result = new LoginResponseDto
        {
            Token = "eyJhbGciOiJIUzI1NiJ9." +
           "eyJuYW1lIjoiRGFuaWVsIiwicm9sZSI6IkFkbWluIn0." +
           "fake-signature"
        };

        // if (!response.IsSuccessStatusCode)
        //     return false;

        // var result =
        //     await response.Content
        //         .ReadFromJsonAsync<LoginResponseDto>();

        await _tokenService.SetTokenAsync(
            result!.Token);

        _authProvider.NotifyUserAuthenticated(
            result.Token);

        return true;
    }

    public async Task Logout()
    {
        await _tokenService.RemoveTokenAsync();

        _authProvider.NotifyUserLoggedOut();
    }
}