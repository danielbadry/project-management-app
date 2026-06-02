namespace AppHost.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null)
        {
            return BadRequest("No login request provided");
        }

        // TODO: Replace with real authentication logic
        return Ok(new
        {
            Token = "eyJhbGciOiJIUzI1NiJ9." +
                "eyJuYW1lIjoiRGFuaWVsIiwicm9sZSI6IkFkbWluIn0." +
                "fake-signature"
        });
    }
}

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
