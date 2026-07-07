using AppHost.ApiService.Data;
using AppHost.ApiService.Entities.Auth;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.Register;

namespace AppHost.ApiService.Services.Auth;

public class RegisterService : IRegisterService
{
    private readonly AppDbContext _dbContext;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterService(
        AppDbContext dbContext,
        IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
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

        var email = request.Email.Trim();
        var username = request.Username.Trim();

        var emailExists = await _dbContext.Users
            .AnyAsync(x => x.Email == email);

        if (emailExists)
        {
            return ApiResponse<RegisterResponseDto>.Fail(
                "Email already exists");
        }

        var usernameExists = await _dbContext.Users
            .AnyAsync(x => x.Username == username);

        if (usernameExists)
        {
            return ApiResponse<RegisterResponseDto>.Fail(
                "Username already exists");
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _dbContext.Users.Add(user);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            return ApiResponse<RegisterResponseDto>.Fail(
                "Email or username already exists");
        }

        return ApiResponse<RegisterResponseDto>.Success(
            new RegisterResponseDto
            {
                Token = _jwtTokenService.CreateToken(user)
            },
            "User registered successfully");
    }
}
