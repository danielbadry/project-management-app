namespace AppHost.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Dtos.Register;


[ApiController]
[Route("api/register")]
public class RegisterController : ControllerBase
{

    [HttpPost]
    public IActionResult RegisterUser([FromBody] RegisterFormDataDto request)
    {

        if (request == null)
        {
            return BadRequest("No register request provided");
        }

        if (request.ConfirmPassword != request.Password)
        {
            return BadRequest("password is wrong");
        }

        return Ok(new ApiResponse<RegisterResponseDto>
        {
            Message = "success login",
            Response = new RegisterResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiJ9." +
                "eyJuYW1lIjoiRGFuaWVsIiwicm9sZSI6IkFkbWluIn0." +
                "fake-signature"
            }
        });
    }

}