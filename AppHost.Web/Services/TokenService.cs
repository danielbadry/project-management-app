using AppHost.Web.Services;

public class TokenService
{
    private readonly LocalStorageService _localStorage;

    public TokenService(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SetTokenAsync(string token)
    {
        await _localStorage.SetItemAsync("token", token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("token");
    }

    public async Task RemoveTokenAsync()
    {
        await _localStorage.RemoveItemAsync("token");
    }
}
