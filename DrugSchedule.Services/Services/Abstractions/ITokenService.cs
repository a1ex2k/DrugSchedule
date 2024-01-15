using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface ITokenService
{
    Task<OneOf<TokenModel, InvalidInput>> RefreshTokensAsync(TokenModel tokenModel, CancellationToken cancellationToken = default);

    Task<OneOf<TokenModel, InvalidInput>> CreateTokensAsync(TokenCreateParams parameters, CancellationToken cancellationToken = default);

    Task<bool> RevokeRefreshTokenAsync(Guid userGuid, string refreshToken, CancellationToken cancellationToken = default);
}