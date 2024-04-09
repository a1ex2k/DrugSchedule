using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public interface IApiClient
{
    Task<ApiCallResult<TResponse>> PostAsync<TResponse>(string uri, CancellationToken cancellationToken = default);

    Task<ApiCallResult<TResponse>> PostAsync<TRequest, TResponse>(
        TRequest requestBody, string uri, CancellationToken cancellationToken = default);

    Task<ApiCallResult> PostAsync<TRequest>(
        TRequest requestBody, string uri, CancellationToken cancellationToken = default);


    Task<ApiCallResult<TResponse>> PostAsync<TResponse>(
        MultipartFormDataContent formDataContent, string uri, CancellationToken cancellationToken = default);

    Task<ApiCallResult> LogInAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

    Task LogOutAsync(CancellationToken cancellationToken = default);

    Task CheckClientAuthAsync(CancellationToken cancellationToken = default);
}