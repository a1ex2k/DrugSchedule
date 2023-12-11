using System;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IIdentityRepository
{
    public Task<UserIdentity?> GetUserIdentityAsync(string username);

    public Task<UserIdentity?> GetUserIdentityAsync(string username, string password);

    public Task<UserIdentity?> GetUserIdentityAsync(Guid userGuid);

    public Task<bool> IsUsernameUsedAsync(string username);

    public Task<bool> IsEmailUsedAsync(string email);

    public Task<UserIdentity> CreateUserIdentityAsync(NewUserIdentity newIdentity);

    public Task<bool> UpdatePasswordAsync(Guid userGuid, string oldPassword, string newPassword);
}