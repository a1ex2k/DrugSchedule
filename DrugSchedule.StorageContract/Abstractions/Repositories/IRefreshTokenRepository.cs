using System;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.UserStorage;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IRefreshTokenRepository
{
    public Task<RefreshTokenEntry?> AddRefreshTokenAsync(RefreshTokenEntry refreshTokenEntry, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveRefreshTokenAsync(Guid userGuid, string refreshToken, CancellationToken cancellationToken = default);
    
    public Task<RefreshTokenEntry?> GetRefreshTokenEntryAsync(Guid userGuid, string refreshToken, CancellationToken cancellationToken = default);
}