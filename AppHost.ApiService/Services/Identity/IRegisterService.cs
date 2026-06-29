using Shared.Dtos;
using Shared.Dtos.Register;

namespace AppHost.ApiService.Services.Identity;

public interface IRegisterService
{
    Task<ApiResponse<RegisterResponseDto>> RegisterAsync(
        RegisterFormDataDto request);
}