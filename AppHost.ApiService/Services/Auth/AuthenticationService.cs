using AppHost.ApiService.Data;
using AppHost.ApiService.Dtos;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.Login;

namespace AppHost.ApiService.Services.Auth;

public class AuthenticationService : IAuthenticationService
{
    private const int MaximumFailedLoginAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    private readonly AppDbContext _dbContext;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthenticationService(
        AppDbContext dbContext,
        IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        if (request == null)
        {
            return ApiResponse<LoginResponseDto>.Fail(
                "Username or Password is not correct ");
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x =>
                x.Username == request.Username.Trim() &&
                x.IsActive);

        if (user is null)
        {
            return InvalidCredentials();
        }

        var now = DateTimeOffset.UtcNow;

        if (user.LockoutEndUtc > now)
        {
            return InvalidCredentials();
        }

        if (user.LockoutEndUtc is not null)
        {
            user.LockoutEndUtc = null;
            user.FailedLoginAttempts = 0;
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;
            user.UpdatedAtUtc = now;

            if (user.FailedLoginAttempts >= MaximumFailedLoginAttempts)
            {
                user.LockoutEndUtc = now.Add(LockoutDuration);
            }

            await _dbContext.SaveChangesAsync();
            return InvalidCredentials();
        }

        if (user.FailedLoginAttempts != 0 || user.LockoutEndUtc is not null)
        {
            user.FailedLoginAttempts = 0;
            user.LockoutEndUtc = null;
            user.UpdatedAtUtc = now;
            await _dbContext.SaveChangesAsync();
        }

        return ApiResponse<LoginResponseDto>.Success(new LoginResponseDto
        {
            Token = _jwtTokenService.CreateToken(user)
        });

    }

    private static ApiResponse<LoginResponseDto> InvalidCredentials() =>
        ApiResponse<LoginResponseDto>.Fail(
            "Username or password is incorrect, or the account is temporarily unavailable.");

}
