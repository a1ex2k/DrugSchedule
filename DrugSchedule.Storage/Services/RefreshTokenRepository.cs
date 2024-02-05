using DrugSchedule.Storage.Extensions;
using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RefreshTokenEntry = DrugSchedule.Storage.Data.Entities.RefreshTokenEntry;

namespace DrugSchedule.Storage.Services;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<RefreshTokenRepository> _logger;

    public RefreshTokenRepository(DrugScheduleContext dbContext, UserManager<IdentityUser> userManager, ILogger<RefreshTokenRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Contract.RefreshTokenEntry?> AddRefreshTokenAsync(Contract.RefreshTokenEntry refreshTokenEntry, CancellationToken cancellationToken = default)
    {
        var entry = new RefreshTokenEntry
        {
            IdentityUserGuid = refreshTokenEntry.UserGuid.ToString(),
            RefreshToken = refreshTokenEntry.RefreshToken,
            RefreshTokenExpiryTime = refreshTokenEntry.RefreshTokenExpiryTime,
            ClientInfo = refreshTokenEntry.ClientInfo
        };

        await _dbContext.RefreshTokens.AddAsync(entry, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? entry.ToContractModel() : null;
    }


    public async Task<Contract.RemoveOperationResult> RemoveRefreshTokenAsync(Guid userGuid, string refreshToken, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.RefreshTokens
                                           .Where(rte => rte.IdentityUserGuid == userGuid.ToString())
                                           .Where(rte => rte.RefreshToken == refreshToken)
                                           .ExecuteDeleteAsync(cancellationToken);
        return result > 0 ? Contract.RemoveOperationResult.Removed : Contract.RemoveOperationResult.NotFound;
    }


    public async Task<Contract.RefreshTokenEntry?> GetRefreshTokenEntryAsync(Guid userGuid, string refreshToken, CancellationToken cancellationToken = default)
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
            .FirstOrDefaultAsync(cancellationToken);

        return result is null
            ? null
            : new Contract.RefreshTokenEntry
            {
                UserGuid = userGuid,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = result.RefreshTokenExpiryTime,
                ClientInfo = result.ClientInfo
            };
    }
}