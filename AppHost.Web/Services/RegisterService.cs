using Shared.Dtos;
using Shared.Dtos.Register;
namespace AppHost.Web.Services;

public class RegisterService(IHttpClientFactory factory, ILogger<RegisterService> logger)
{
    private readonly string registerUrl = "api/register";
    private readonly HttpClient _http = factory.CreateClient("ApiClient");
    private readonly ILogger<RegisterService> _logger = logger;

    public async Task<bool> RegisterUser(RegisterFormDataDto registerDto)
    {
        try
        {
            using var response = await _http.PostAsJsonAsync(registerUrl, registerDto);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Register failed with status code {StatusCode}. Response body: {Body}",
                    response.StatusCode,
                    responseBody);
                return false;
            }

            var result =
                await response.Content
                    .ReadFromJsonAsync<ApiResponse<RegisterResponseDto>>();

            if (result is null || result.Response is null)
            {
                _logger.LogWarning(
                    "Register response body could not be parsed as RegisterResponseDto. Body: {Body}",
                    responseBody);
                return false;
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for register user");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected register user error");
            return false;
        }

    }

}
