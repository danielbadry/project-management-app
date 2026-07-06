using System.Net.Http.Headers;
using System.Text.Json;
using Shared.Dtos;

namespace AppHost.Web.Services;

public class ApiClient(
    IHttpClientFactory factory,
    ILogger<ApiClient> logger,
    TokenService tokenService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _http = factory.CreateClient("ApiClient");
    private readonly ILogger<ApiClient> _logger = logger;
    private readonly TokenService _tokenService = tokenService;

    public async Task<ServiceResult<TResponse>> GetAsync<TResponse>(
        string url,
        string operationName,
        string fallbackErrorMessage = "Request failed. Please try again.")
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "{OperationName} failed with status code {StatusCode}. Response body: {Body}",
                    operationName,
                    response.StatusCode,
                    responseBody);

                return ServiceResult<TResponse>.Failure(
                    GetErrorMessage(responseBody, fallbackErrorMessage));
            }

            var apiResponse =
                JsonSerializer.Deserialize<ApiResponse<TResponse>>(
                    responseBody,
                    JsonOptions);

            if (apiResponse is null || apiResponse.Response is null)
            {
                _logger.LogWarning(
                    "{OperationName} response body could not be parsed as ApiResponse<{ResponseType}>. Body: {Body}",
                    operationName,
                    typeof(TResponse).Name,
                    responseBody);

                return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
            }

            return ServiceResult<TResponse>.Success(apiResponse.Response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure("Could not reach the API. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected API error for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
        }
    }

    public async Task<ServiceResult<TResponse>> PostAsync<TResponse>(
        string url,
        object request,
        string operationName,
        string fallbackErrorMessage = "Request failed. Please try again.")
    {
        try
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(request, options: JsonOptions)
            };
            using var response = await SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "{OperationName} failed with status code {StatusCode}. Response body: {Body}",
                    operationName,
                    response.StatusCode,
                    responseBody);

                return ServiceResult<TResponse>.Failure(
                    GetErrorMessage(responseBody, fallbackErrorMessage));
            }

            var apiResponse =
                JsonSerializer.Deserialize<ApiResponse<TResponse>>(
                    responseBody,
                    JsonOptions);

            if (apiResponse is null || apiResponse.Response is null)
            {
                _logger.LogWarning(
                    "{OperationName} response body could not be parsed as ApiResponse<{ResponseType}>. Body: {Body}",
                    operationName,
                    typeof(TResponse).Name,
                    responseBody);

                return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
            }

            return ServiceResult<TResponse>.Success(apiResponse.Response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure("Could not reach the API. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected API error for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
        }
    }

    public async Task<ServiceResult<TResponse>> PutAsync<TResponse>(
        string url,
        object request,
        string operationName,
        string fallbackErrorMessage = "Request failed. Please try again.")
    {
        try
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = JsonContent.Create(request, options: JsonOptions)
            };
            using var response = await SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "{OperationName} failed with status code {StatusCode}. Response body: {Body}",
                    operationName,
                    response.StatusCode,
                    responseBody);

                return ServiceResult<TResponse>.Failure(
                    GetErrorMessage(responseBody, fallbackErrorMessage));
            }

            var apiResponse =
                JsonSerializer.Deserialize<ApiResponse<TResponse>>(
                    responseBody,
                    JsonOptions);

            if (apiResponse is null || apiResponse.Response is null)
            {
                _logger.LogWarning(
                    "{OperationName} response body could not be parsed as ApiResponse<{ResponseType}>. Body: {Body}",
                    operationName,
                    typeof(TResponse).Name,
                    responseBody);

                return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
            }

            return ServiceResult<TResponse>.Success(apiResponse.Response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure("Could not reach the API. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected API error for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
        }
    }

    public async Task<ServiceResult<TResponse>> DeleteAsync<TResponse>(
        string url,
        string operationName,
        string fallbackErrorMessage = "Request failed. Please try again.")
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            using var response = await SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "{OperationName} failed with status code {StatusCode}. Response body: {Body}",
                    operationName,
                    response.StatusCode,
                    responseBody);

                return ServiceResult<TResponse>.Failure(
                    GetErrorMessage(responseBody, fallbackErrorMessage));
            }

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TResponse>>(
                responseBody,
                JsonOptions);

            if (apiResponse is null || apiResponse.Response is null)
            {
                _logger.LogWarning(
                    "{OperationName} response body could not be parsed as ApiResponse<{ResponseType}>. Body: {Body}",
                    operationName,
                    typeof(TResponse).Name,
                    responseBody);

                return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
            }

            return ServiceResult<TResponse>.Success(apiResponse.Response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure("Could not reach the API. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected API error for {OperationName}", operationName);
            return ServiceResult<TResponse>.Failure(fallbackErrorMessage);
        }
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        var token = await _tokenService.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await _http.SendAsync(request);
    }

    private static string GetErrorMessage(string responseBody, string fallbackErrorMessage)
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            return fallbackErrorMessage;
        }

        try
        {
            using var json = JsonDocument.Parse(responseBody);
            var root = json.RootElement;

            if (root.ValueKind == JsonValueKind.String)
            {
                return root.GetString() ?? fallbackErrorMessage;
            }

            if (TryGetStringProperty(root, "message", out var message))
            {
                return message;
            }

            if (TryGetValidationError(root, out var validationError))
            {
                return validationError;
            }

            if (TryGetStringProperty(root, "title", out var title))
            {
                return title;
            }
        }
        catch (JsonException)
        {
            return responseBody;
        }

        return fallbackErrorMessage;
    }

    private static bool TryGetStringProperty(
        JsonElement root,
        string propertyName,
        out string value)
    {
        value = "";

        if (root.TryGetProperty(propertyName, out var property) &&
            property.ValueKind == JsonValueKind.String)
        {
            value = property.GetString() ?? "";
            return !string.IsNullOrWhiteSpace(value);
        }

        return false;
    }

    private static bool TryGetValidationError(JsonElement root, out string value)
    {
        value = "";

        if (!root.TryGetProperty("errors", out var errors) ||
            errors.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        foreach (var property in errors.EnumerateObject())
        {
            if (property.Value.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var error in property.Value.EnumerateArray())
            {
                if (error.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                value = error.GetString() ?? "";
                return !string.IsNullOrWhiteSpace(value);
            }
        }

        return false;
    }
}
