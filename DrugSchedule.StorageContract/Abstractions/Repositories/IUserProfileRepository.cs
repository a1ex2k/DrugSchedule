using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserProfileRepository
{
    public Task<UserProfile?> GetUserProfileAsync(long id);

    public Task<List<UserProfile>> GetUserProfilesAsync(List<long> ids);

    public Task<UserProfile?> GetUserProfilesByIdentityGuidAsync(Guid guid);

    public Task<UserProfile> CreateUserProfileAsync(UserProfile newUserProfile);

    public Task<UserProfile> UpdateUserProfileAsync(UserProfile userProfile);
}