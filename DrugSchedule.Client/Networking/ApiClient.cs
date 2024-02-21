﻿using System.Net;
using System.Net.Http.Json;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking.Exceptions;

namespace DrugSchedule.Client.Networking;

public class ApiClient
{
    private readonly ITokenStorage _tokenStorage;
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient, ITokenStorage tokenStorage)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
    }


    public async Task<ApiCallResult<TResponse>> PostAsync<TRequest, TResponse>(
        TRequest requestBody, string uri, CancellationToken cancellationToken = default)
    {
        await AssertTokensAsync(cancellationToken);
        var response = await _httpClient.PostAsJsonAsync(uri, requestBody, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await RefreshTokensAsync(cancellationToken);
            response = await _httpClient.PostAsJsonAsync(uri, requestBody, cancellationToken);
        }
        return await BuildResultAsync<TResponse>(response, cancellationToken);
    }


    public async Task<ApiCallResult<TokenDto>> LogInAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(EndpointsPaths.Auth_Login, loginDto, cancellationToken);
        var result = await BuildResultAsync<TokenDto>(response, cancellationToken);

        if (result.IsOk)
        {
            SetHttpClientToken(result.Result.AccessToken);
        }

        return result;
    }

    public async Task LogOutAsync()
    {
        await _tokenStorage.RemoveTokensAsync();
        SetHttpClientToken(null);
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
            case HttpStatusCode.Unauthorized:
                {
                    throw new UnauthorizedException();
                }
            default:
                throw new InvalidDataException("Response not recognized");
        }
    }


    private async Task AssertTokensAsync(CancellationToken cancellationToken)
    {
        if (!await _tokenStorage.IsSetAsync())
        {
            throw new UnauthorizedException();
        }

        SetHttpClientToken(_tokenStorage.GetAccessToken());
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
            throw new UnauthorizedException();
        }

        var tokenResponseDto = await response.Content.ReadFromJsonAsync<TokenDto>(cancellationToken);
        await _tokenStorage.WriteTokensAsync(tokenResponseDto!.AccessToken, tokenResponseDto.RefreshToken);
    }


    private void SetHttpClientToken(string? authToken)
    {
        _httpClient.DefaultRequestHeaders.Remove("Authorization");
        if (authToken == null) return;
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);
    }

}