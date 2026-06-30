using AppHost.ApiService.Dtos;
using Shared.Dtos;
using Shared.Dtos.Login;

namespace AppHost.ApiService.Services.Identity;

public interface IAuthenticationService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(
        LoginRequestDto request);
}