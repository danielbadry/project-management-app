namespace AppHost.ApiService.Controllers;

using AppHost.ApiService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.User;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActiveUsers()
    {
        var users = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.IsActive)
            .OrderBy(user => user.Name)
            .Select(user => new UserOptionDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username
            })
            .ToListAsync();

        return Ok(ApiResponse<List<UserOptionDto>>.Success(
            users,
            "Users loaded successfully"));
    }
}
