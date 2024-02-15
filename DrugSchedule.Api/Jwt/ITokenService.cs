using DrugSchedule.Services.Errors;
using DrugSchedule.Services.Models;
using OneOf;
using OneOf.Types;

namespace DrugSchedule.Services.Services.Abstractions;

public interface ITokenService
{
    Task<OneOf<TokenModel, InvalidInput>> RefreshTokensAsync(TokenModel tokenModel, CancellationToken cancellationToken = default);

    Task<OneOf<TokenModel, InvalidInput>> CreateTokensAsync(TokenCreateParams parameters, CancellationToken cancellationToken = default);

    Task<bool> RevokeRefreshTokenAsync(Guid userGuid, string refreshToken, CancellationToken cancellationToken = default);
}