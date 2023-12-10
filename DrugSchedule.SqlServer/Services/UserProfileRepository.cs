using DrugSchedule.SqlServer.Data;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.SqlServer.Services;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<UserProfileRepository> _logger;

    public UserProfileRepository(DrugScheduleContext dbContext, ILogger<UserProfileRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public Task<UserProfile?> GetUserProfileByIdAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task<List<UserProfile>> GetUserProfilesWithIdsAsync(List<long> ids)
    {
        throw new NotImplementedException();
    }


    public Task<UserProfile> CreateUserProfileAsync(UserProfile newUserProfile)
    {
        throw new NotImplementedException();
    }


    public Task<UserProfile> UpdateUserProfileAsync(UserProfile newUserProfile)
    {
        throw new NotImplementedException();
    }
}