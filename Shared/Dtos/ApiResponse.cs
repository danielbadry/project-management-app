namespace Shared.Dtos;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Response { get; set; }

    public List<string> Errors { get; set; } = [];

    public static ApiResponse<T> Success(T response, string message = "Success")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Response = response
        };
    }

    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Response = default
        };
    }

    public static ApiResponse<T> Fail(List<string> errors, string message = "Validation failed")
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors,
            Response = default
        };
    }
}