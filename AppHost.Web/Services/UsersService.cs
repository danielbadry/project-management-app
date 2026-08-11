namespace AppHost.Web.Services;

using Shared.Dtos.User;

public class UsersService(ApiClient apiClient)
{
    public Task<ServiceResult<List<UserOptionDto>>> GetUsersAsync() =>
        apiClient.GetAsync<List<UserOptionDto>>(
            "api/users",
            "load users",
            "Loading users failed.");
}
