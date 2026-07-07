namespace AppHost.ApiService.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using AppHost.ApiService.Dtos;
using AppHost.ApiService.Services.Auth;
using Shared.Dtos;
using Shared.Dtos.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    [HttpPost]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authenticationService.LoginAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        else
        {
            return Ok(result);
        }
    }

    [Authorize]
    [HttpGet("session")]
    public IActionResult GetSession()
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!int.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var session = new AuthenticatedUserDto
        {
            UserId = userId,
            Name = User.FindFirstValue(JwtRegisteredClaimNames.Name) ??
                   User.Identity?.Name ??
                   string.Empty,
            Username = User.FindFirstValue(JwtRegisteredClaimNames.UniqueName) ??
                       User.FindFirstValue(ClaimTypes.Name) ??
                       string.Empty,
            Email = User.FindFirstValue(ClaimTypes.Email) ??
                    User.FindFirstValue(JwtRegisteredClaimNames.Email) ??
                    string.Empty,
            Role = User.FindFirstValue(ClaimTypes.Role) ??
                   User.FindFirstValue("role") ??
                   string.Empty
        };

        return Ok(ApiResponse<AuthenticatedUserDto>.Success(session));
    }
}
