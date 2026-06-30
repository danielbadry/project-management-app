using AppHost.ApiService.Entities.Identity;

namespace AppHost.ApiService.Services.Identity;

public interface IJwtTokenService
{
    string CreateToken(User user);
}
