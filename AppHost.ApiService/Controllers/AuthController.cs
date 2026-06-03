namespace AppHost.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using AppHost.ApiService.Dtos;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (request == null)
        {
            return BadRequest("No login request provided");
        }

        return Ok(new
        {
            Token = "eyJhbGciOiJIUzI1NiJ9." +
                "eyJuYW1lIjoiRGFuaWVsIiwicm9sZSI6IkFkbWluIn0." +
                "fake-signature"
        });
    }
}
