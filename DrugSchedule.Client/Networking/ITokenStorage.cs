using Blazored.LocalStorage;

namespace DrugSchedule.Client.Networking;

public interface ITokenStorage
{
    ValueTask<bool> IsSetAsync();

    string GetAccessToken();

    string GetRefreshToken();

    ValueTask RemoveTokensAsync();

    ValueTask WriteTokensAsync(string accessToken, string refreshToken);
}

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
        return _accessToken == null || _refreshToken == null;
    }

    public string GetAccessToken()
    {
        throw new NotImplementedException();
    }

    public string GetRefreshToken()
    {
        throw new NotImplementedException();
    }

    public ValueTask RemoveTokensAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask WriteTokensAsync(string accessToken, string refreshToken)
    {
        throw new NotImplementedException();
    }
}