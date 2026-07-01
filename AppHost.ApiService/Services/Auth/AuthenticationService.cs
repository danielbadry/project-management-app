using AppHost.ApiService.Data;
using AppHost.ApiService.Dtos;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.Login;

namespace AppHost.ApiService.Services.Auth;

public class AuthenticationService : IAuthenticationService
{
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
                x.Username == request.Username &&
                x.IsActive);

        if (user is not null &&
            BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return ApiResponse<LoginResponseDto>.Success(new LoginResponseDto
            {
                Token = _jwtTokenService.CreateToken(user)
            });
        }

        return ApiResponse<LoginResponseDto>.Fail(
            "Username or Password is not correct ");

    }

}
