using AppHost.ApiService.Data;
using AppHost.ApiService.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.Register;

namespace AppHost.ApiService.Services.Identity;

public class RegisterService : IRegisterService
{
    private readonly AppDbContext _dbContext;

    public RegisterService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(
        RegisterFormDataDto request)
    {
        if (request is null)
        {
            return ApiResponse<RegisterResponseDto>.Fail(
                "No register request provided");
        }

        if (request.Password != request.ConfirmPassword)
        {
            return ApiResponse<RegisterResponseDto>.Fail(
                "Password and confirm password do not match");
        }

        var emailExists = await _dbContext.Users
            .AnyAsync(x => x.Email == request.Email);

        if (emailExists)
        {
            return ApiResponse<RegisterResponseDto>.Fail(
                "Email already exists");
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return ApiResponse<RegisterResponseDto>.Success(
            new RegisterResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiJ9." +
                "eyJuYW1lIjoiRGFuaWVsIiwicm9sZSI6IkFkbWluIn0." +
                "fake-signature"
            },
            "User registered successfully");
    }
}