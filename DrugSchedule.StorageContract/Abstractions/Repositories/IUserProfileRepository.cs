using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserProfileRepository
{
    public Task<UserProfile?> GetUserProfileByIdAsync(int id);

    public Task<List<UserProfile>> GetUserProfilesWithIdsAsync(List<long> ids);

    public Task<UserProfile> CreateUserProfileAsync(UserProfile newUserProfile);

    public Task<UserProfile> UpdateUserProfileAsync(UserProfile newUserProfile);
}