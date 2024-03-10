using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public static class AuthControllerApiClient
{
    public static async Task<ApiCallResult> LoginAsync(this IApiClient client, LoginDto body, CancellationToken cancellationToken = default)
    {
        return await client.LogInAsync(body, cancellationToken);
    }

    public static async Task LogoutAsync(this IApiClient client, CancellationToken cancellationToken = default)
    {
        await client.LogOutAsync(cancellationToken);
    }

    public static async Task<ApiCallResult> RegisterAsync(this IApiClient client, RegisterDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync(body, EndpointsPaths.Auth_Register, cancellationToken);
    }

    public static async Task<ApiCallResult<AvailableUsernameDto>> UsernameAvailableAsync(this IApiClient client, UsernameDto body, CancellationToken cancellationToken = default)
    {
        return await client.PostAsync<UsernameDto, AvailableUsernameDto>(body, EndpointsPaths.Auth_UsernameAvailable, cancellationToken);
    }
}