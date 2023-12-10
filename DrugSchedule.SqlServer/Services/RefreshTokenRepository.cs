using DrugSchedule.SqlServer.Data;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data.UserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.SqlServer.Services;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityContext _identityContext;
    private readonly ILogger<RefreshTokenRepository> _logger;

    public RefreshTokenRepository(IdentityContext dbContext, UserManager<IdentityUser> userManager, ILogger<RefreshTokenRepository> logger)
    {
        _identityContext = dbContext;
        _logger = logger;
    }
    

    public async Task<bool> AddRefreshTokenAsync(RefreshTokenEntry refreshTokenEntry)
    {
        var entry = new Data.Entities.RefreshTokenEntry
        {
            IdentityUserGuid = refreshTokenEntry.UserGuid.ToString(),
            RefreshToken = refreshTokenEntry.RefreshToken,
            RefreshTokenExpiryTime = refreshTokenEntry.RefreshTokenExpiryTime,
            ClientInfo = refreshTokenEntry.ClientInfo
        };

        try
        {
            await _identityContext.RefreshTokens.AddAsync(entry);
            await _identityContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot save Refresh token");
            return false;
        }

        return true;
    }


    public async Task<bool> RemoveRefreshTokenAsync(Guid userGuid, string refreshToken)
    {
        var result = await _identityContext.RefreshTokens
                                           .Where(rte => rte.IdentityUserGuid == userGuid.ToString())
                                           .Where(rte => rte.RefreshToken == refreshToken)
                                           .ExecuteDeleteAsync();

        return result != 0;
    }


    public async Task<bool> IsRefreshTokenExistsAsync(Guid userGuid, string refreshToken)
    {
        var result = await _identityContext.RefreshTokens
                                           .Where(rte => rte.IdentityUserGuid == userGuid.ToString())
                                           .Where(rte => rte.RefreshToken == refreshToken)
                                           .AnyAsync();

        return result;
    }
}