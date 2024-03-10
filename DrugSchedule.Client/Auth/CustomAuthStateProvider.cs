using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace DrugSchedule.Client.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ITokenStorage _tokenStorage;
    private AuthenticationState? _state;
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(ITokenStorage tokenStorage, HttpClient httpClient)
    {
        _tokenStorage = tokenStorage;
        _httpClient = httpClient;
        UpdateStateFromTokensAsync();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_state == null)
        {
            await UpdateStateFromTokensAsync();
        }

        return _state!;
    }

    public async Task UpdateStateFromTokensAsync()
    {
        var isSet = await _tokenStorage.IsSetAsync();
        var identity = new ClaimsIdentity();
        _httpClient.DefaultRequestHeaders.Remove("Authorization");
       
        if (isSet)
        {
            var accessToken = _tokenStorage.GetAccessToken();
            identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        var user = new ClaimsPrincipal(identity);
        _state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}