using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;

namespace DrugSchedule.Client.Networking;

public interface IAuthControllerClient
{
    Task<ApiCallResult> LoginAsync(LoginDto body, CancellationToken cancellationToken = default);

    Task LogoutAsync(CancellationToken cancellationToken = default);

    Task<ApiCallResult> RegisterAsync(RegisterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<AvailableUsernameDto>> UsernameAvailableAsync(UsernameDto body, CancellationToken cancellationToken = default);
}