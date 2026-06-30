namespace AppHost.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using AppHost.ApiService.Dtos;
using Shared.Dtos;
using Shared.Dtos.Login;
using AppHost.ApiService.Services.Identity;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    [HttpPost]
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
}
