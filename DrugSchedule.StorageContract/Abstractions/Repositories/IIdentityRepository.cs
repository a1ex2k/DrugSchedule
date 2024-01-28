using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.UserStorage;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IIdentityRepository
{
    public Task<UserIdentity?> GetUserIdentityAsync(string username, CancellationToken cancellationToken = default);

    public Task<UserIdentity?> GetUserIdentityAsync(string username, string password, CancellationToken cancellationToken = default);

    public Task<UserIdentity?> GetUserIdentityAsync(Guid userGuid, CancellationToken cancellationToken = default);

    public Task<List<UserIdentity>> GetUserIdentitiesAsync(UserIdentityFilter filter, CancellationToken cancellationToken = default);
    
    public Task<bool> IsUsernameUsedAsync(string username, CancellationToken cancellationToken = default);

    public Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken = default);

    public Task<UserIdentity> CreateUserIdentityAsync(NewUserIdentity newIdentity, CancellationToken cancellationToken = default);

    public Task<bool> UpdatePasswordAsync(PasswordUpdate passwordUpdate, CancellationToken cancellationToken = default);
}