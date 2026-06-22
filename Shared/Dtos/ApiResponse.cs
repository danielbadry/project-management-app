namespace Shared.Dtos;

public class ApiResponse<T>
{
    public string Message { get; set; } = "";
    public T Response { get; set; }
}