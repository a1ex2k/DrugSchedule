using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserProfileRepository
{
    public Task<UserProfile?> GetUserProfileAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<UserProfile>> GetUserProfilesAsync(List<long> ids, CancellationToken cancellationToken = default);

    public Task<UserProfile?> GetUserProfileAsync(Guid identityGuid, CancellationToken cancellationToken = default);

    public Task<List<UserProfile>> GetUserProfilesAsync(List<Guid> identityGuids, CancellationToken cancellationToken = default);

    public Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken = default);

    public Task<UserProfile?> UpdateUserProfileAsync(UserProfile userProfile, UserProfileUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<List<UserContact>> GetContactsAsync(long userProfileId, CancellationToken cancellationToken = default);

    public Task<UserContact?> AddOrUpdateContactAsync(UserContact userContact, CancellationToken cancellationToken = default);

    public Task<bool> RemoveContactAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default);

    public Task<List<FileInfo>> GetUserAvatarsInfo(List<Guid> filesGuids, CancellationToken cancellationToken = default);
}