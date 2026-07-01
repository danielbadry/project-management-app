namespace AppHost.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Register;
using AppHost.ApiService.Services.Auth;


[ApiController]
[Route("api/register")]
public class RegisterController : ControllerBase
{
    private readonly IRegisterService _registerService;

    public RegisterController(IRegisterService registerService)
    {
        _registerService = registerService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterFormDataDto request)
    {
        var result = await _registerService.RegisterAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}