using Blazored.LocalStorage;

namespace DrugSchedule.Client.Auth;

public class TokenStorage : ITokenStorage
{
    private string? _accessToken;
    private string? _refreshToken;

    private readonly ILocalStorageService _localStorage;

    private const string LocalStorageAccessTokenKey = "access-token";
    private const string LocalStorageRefreshTokenKey = "refresh-token";

    public TokenStorage(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    private async ValueTask ReadFromLocalStorageIfNotSet()
    {
        if (_accessToken == null || _refreshToken == null)
        {
            _accessToken = await _localStorage.GetItemAsStringAsync(LocalStorageAccessTokenKey);
            _refreshToken = await _localStorage.GetItemAsStringAsync(LocalStorageRefreshTokenKey);
        }
    }

    public async ValueTask<bool> IsSetAsync()
    {
        await ReadFromLocalStorageIfNotSet();
        return !string.IsNullOrWhiteSpace(_accessToken) && !string.IsNullOrWhiteSpace(_refreshToken);
    }

    public string GetAccessToken()
    {
        return _accessToken!;
    }

    public string GetRefreshToken()
    {
        return _refreshToken!;
    }

    public async ValueTask RemoveTokensAsync()
    {
        _refreshToken = null;
        _accessToken = null;
        await _localStorage.RemoveItemsAsync(new[] { LocalStorageAccessTokenKey, LocalStorageRefreshTokenKey });
    }

    public async ValueTask WriteTokensAsync(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
        await _localStorage.SetItemAsStringAsync(LocalStorageAccessTokenKey, accessToken);
        await _localStorage.SetItemAsStringAsync(LocalStorageRefreshTokenKey, refreshToken);
    }
}