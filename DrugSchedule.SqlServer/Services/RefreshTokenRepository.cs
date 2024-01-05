using DrugSchedule.SqlServer.Data;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data.UserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.SqlServer.Services;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<RefreshTokenRepository> _logger;

    public RefreshTokenRepository(DrugScheduleContext dbContext, UserManager<IdentityUser> userManager, ILogger<RefreshTokenRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task AddRefreshTokenAsync(RefreshTokenEntry refreshTokenEntry)
    {
        var entry = new Data.Entities.RefreshTokenEntry
        {
            IdentityUserGuid = refreshTokenEntry.UserGuid.ToString(),
            RefreshToken = refreshTokenEntry.RefreshToken,
            RefreshTokenExpiryTime = refreshTokenEntry.RefreshTokenExpiryTime,
            ClientInfo = refreshTokenEntry.ClientInfo
        };

        await _dbContext.RefreshTokens.AddAsync(entry);
        await _dbContext.SaveChangesAsync();
    }


    public async Task<bool> RemoveRefreshTokenAsync(Guid userGuid, string refreshToken)
    {
        var result = await _dbContext.RefreshTokens
                                           .Where(rte => rte.IdentityUserGuid == userGuid.ToString())
                                           .Where(rte => rte.RefreshToken == refreshToken)
                                           .ExecuteDeleteAsync();

        return result != 0;
    }


    public async Task<RefreshTokenEntry?> GetRefreshTokenEntryAsync(Guid userGuid, string refreshToken)
    {
        var result = await _dbContext.RefreshTokens
            .AsNoTracking()
            .Where(rte => rte.IdentityUserGuid == userGuid.ToString())
            .Where(rte => rte.RefreshToken == refreshToken)
            .Select(rte => new
            {
                RefreshTokenExpiryTime = rte.RefreshTokenExpiryTime,
                ClientInfo = rte.ClientInfo
            })
            .FirstOrDefaultAsync();

        return result is null
            ? null
            : new RefreshTokenEntry
            {
                UserGuid = userGuid,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = result.RefreshTokenExpiryTime,
                ClientInfo = result.ClientInfo
            };
    }
}