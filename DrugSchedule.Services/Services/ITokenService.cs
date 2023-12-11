using DrugSchedule.BusinessLogic.Auth;
namespace DrugSchedule.BusinessLogic.Services;

public interface ITokenService
{
    Task<TokenModel?> RefreshTokensAsync(TokenModel tokenModel);

    Task<TokenModel?> CreateTokensAsync(Guid userGuid, long userProfileId, string? clientInfo);

    Task<bool> RevokeRefreshTokenAsync(Guid userGuid, string refreshToken);
}