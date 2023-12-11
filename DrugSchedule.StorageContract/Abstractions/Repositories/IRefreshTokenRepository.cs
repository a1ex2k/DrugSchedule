using System;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data.UserStorage;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IRefreshTokenRepository
{
    public Task<bool> AddRefreshTokenAsync(RefreshTokenEntry refreshTokenEntry);

    public Task<bool> RemoveRefreshTokenAsync(Guid userGuid, string refreshToken);
    
    public Task<RefreshTokenEntry?> GetRefreshTokenEntryAsync(Guid userGuid, string refreshToken);
}