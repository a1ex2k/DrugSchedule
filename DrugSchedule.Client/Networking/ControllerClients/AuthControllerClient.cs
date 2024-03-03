using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Networking;

public class AuthControllerClient : IAuthControllerClient
{
    private readonly IApiClient _client;

    public AuthControllerClient(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiCallResult> LoginAsync(LoginDto body, CancellationToken cancellationToken = default)
    {
        return await _client.LogInAsync(body, cancellationToken);
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        await _client.LogOutAsync(cancellationToken);
    }

    public async Task<ApiCallResult> RegisterAsync(RegisterDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(body, EndpointsPaths.Auth_Register, cancellationToken);
    }

    public async Task<ApiCallResult<AvailableUsernameDto>> UsernameAvailableAsync(UsernameDto body, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync<UsernameDto, AvailableUsernameDto>(body, EndpointsPaths.Auth_UsernameAvailable, cancellationToken);
    }
}