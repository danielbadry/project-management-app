using Shared.Dtos;
using Shared.Dtos.Register;
namespace AppHost.Web.Services;

public class RegisterService(HttpClient http, ILogger<RegisterService> logger)
{
    private readonly string registerUrl = "api/register";
    private readonly HttpClient _http = http;
    private readonly ILogger _logger = logger;

    public async Task<bool> RegisterUser(RegisterFormDataDto registerDto)
    {
        try
        {
            using var response = await _http.PostAsJsonAsync(registerUrl, registerDto);

            _logger.LogError("response@@@@@@@", response);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            _logger.LogError("response@@@@@@@", response);
            var result =
                await response.Content
                    .ReadFromJsonAsync<ApiResponse<RegisterResponseDto>>();
            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("HttpRequestException: error in register user", ex);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception:error in register user", ex);
            return false;
        }

    }

}