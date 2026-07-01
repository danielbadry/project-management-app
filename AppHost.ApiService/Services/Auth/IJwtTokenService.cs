using AppHost.ApiService.Entities.Auth;

namespace AppHost.ApiService.Services.Auth;

public interface IJwtTokenService
{
    string CreateToken(User user);
}
