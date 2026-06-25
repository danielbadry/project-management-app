namespace AppHost.Web.Services;

public sealed record ServiceResult<T>(bool Succeeded, T? Data, string? ErrorMessage)
{
    public static ServiceResult<T> Success(T data) => new(true, data, null);

    public static ServiceResult<T> Failure(string errorMessage) => new(false, default, errorMessage);
}

public sealed record ServiceResult(bool Succeeded, string? ErrorMessage)
{
    public static ServiceResult Success() => new(true, null);

    public static ServiceResult Failure(string errorMessage) => new(false, errorMessage);
}
