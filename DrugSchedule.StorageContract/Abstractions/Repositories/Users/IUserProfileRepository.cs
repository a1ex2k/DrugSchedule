﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserProfileRepository
{
    public Task<UserProfile?> GetUserProfileAsync(long id, bool withAvatar, CancellationToken cancellationToken = default);

    public Task<bool> DoesUserProfileExistsAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<UserProfile>> GetUserProfilesAsync(List<long> ids, bool withAvatar, CancellationToken cancellationToken = default);

    public Task<UserProfile?> GetUserProfileAsync(Guid identityGuid, bool withAvatar, CancellationToken cancellationToken = default);

    public Task<List<UserProfile>> GetUserProfilesAsync(List<Guid> identityGuids, bool withAvatar, CancellationToken cancellationToken = default);

    public Task<UserProfile?> CreateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken = default);

    public Task<UserProfile?> UpdateUserProfileAsync(UserProfile userProfile, UserProfileUpdateFlags updateFlags, CancellationToken cancellationToken = default);
}