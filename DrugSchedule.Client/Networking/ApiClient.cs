using System.Net;
using System.Net.Http.Json;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Auth;

namespace DrugSchedule.Client.Networking;

public class ApiClient : IApiClient
{
    private readonly ITokenStorage _tokenStorage;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient, ITokenStorage tokenStorage, CustomAuthStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<ApiCallResult<TResponse>> PostAsync<TResponse>(string uri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(uri, null, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await RefreshTokensAsync(cancellationToken);
            response = await _httpClient.PostAsync(uri, null, cancellationToken);
        }
        return await BuildResultAsync<TResponse>(response, cancellationToken);
    }

    public async Task<ApiCallResult<TResponse>> PostAsync<TRequest, TResponse>(
        TRequest requestBody, string uri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(uri, requestBody, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await RefreshTokensAsync(cancellationToken);
            response = await _httpClient.PostAsJsonAsync(uri, requestBody, cancellationToken);
        }
        return await BuildResultAsync<TResponse>(response, cancellationToken);
    }


    public async Task<ApiCallResult> PostAsync<TRequest>(
        TRequest requestBody, string uri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(uri, requestBody, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await RefreshTokensAsync(cancellationToken);
            response = await _httpClient.PostAsJsonAsync(uri, requestBody, cancellationToken);
        }
        return await BuildResultAsync(response, cancellationToken);
    }

    public async Task<ApiCallResult<TResponse>> PostAsync<TResponse>(
        MultipartFormDataContent formDataContent, string uri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(uri, formDataContent, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await RefreshTokensAsync(cancellationToken);
            response = await _httpClient.PostAsync(uri, formDataContent, cancellationToken);
        }
        return await BuildResultAsync<TResponse>(response, cancellationToken);
    }


    public async Task<ApiCallResult> LogInAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(EndpointsPaths.Auth_Login, loginDto, cancellationToken);
        var result = await BuildResultAsync<TokenDto>(response, cancellationToken);
        if (result.IsOk)
        {
            await _tokenStorage.WriteTokensAsync(result.ResponseDto!.AccessToken, result.ResponseDto.RefreshToken);
            await _authStateProvider.UpdateStateFromTokensAsync();
        }

        return result;
    }

    public async Task LogOutAsync(CancellationToken cancellationToken = default)
    {
        await _httpClient.PostAsJsonAsync(EndpointsPaths.Auth_RevokeTokens, new TokenDto
        {
            RefreshToken = _tokenStorage.GetRefreshToken(),
            AccessToken = _tokenStorage.GetAccessToken()
        },  cancellationToken);

        await _tokenStorage.RemoveTokensAsync();
        await _authStateProvider.UpdateStateFromTokensAsync();
    }


    private async Task<ApiCallResult<TResponse>> BuildResultAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    var deserializedObject = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
                    return new ApiCallResult<TResponse>(deserializedObject!);
                }
            case HttpStatusCode.NotFound:
                {
                    var notFound = await response.Content.ReadFromJsonAsync<NotFoundDto>(cancellationToken);
                    return new ApiCallResult<TResponse>(notFound!);
                }
            case HttpStatusCode.BadRequest:
                {
                    var invalidInput = await response.Content.ReadFromJsonAsync<InvalidInputDto>(cancellationToken);
                    return new ApiCallResult<TResponse>(invalidInput!);
                }
            default:
                throw new InvalidDataException("Response not recognized");
        }
    }


    private async Task<ApiCallResult> BuildResultAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            {
                return new ApiCallResult();
            }
            case HttpStatusCode.NotFound:
            {
                var notFound = await response.Content.ReadFromJsonAsync<NotFoundDto>(cancellationToken);
                return new ApiCallResult(notFound!);
            }
            case HttpStatusCode.BadRequest:
            {
                var invalidInput = await response.Content.ReadFromJsonAsync<InvalidInputDto>(cancellationToken);
                return new ApiCallResult(invalidInput!);
            }
            default:
                throw new InvalidDataException("Response not recognized");
        }
    }


    private async Task RefreshTokensAsync(CancellationToken cancellationToken)
    {
        var tokenDto = new TokenDto
        {
            AccessToken = _tokenStorage.GetAccessToken(),
            RefreshToken = _tokenStorage.GetRefreshToken()
        };

        var response = await _httpClient.PostAsJsonAsync(EndpointsPaths.Auth_RefreshToken, tokenDto, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            await _tokenStorage.RemoveTokensAsync();
            await _authStateProvider.UpdateStateFromTokensAsync();
            return;
        }

        var tokenResponseDto = await response.Content.ReadFromJsonAsync<TokenDto>(cancellationToken);
        await _tokenStorage.WriteTokensAsync(tokenResponseDto!.AccessToken, tokenResponseDto.RefreshToken);
        await _authStateProvider.UpdateStateFromTokensAsync();
    }
}