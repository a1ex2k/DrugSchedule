using DrugSchedule.SqlServer.Data;
using DrugSchedule.SqlServer.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace DrugSchedule.SqlServer.Services;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly DrugScheduleContext _dbContext;

    public UserProfileRepository(DrugScheduleContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Contract.UserProfile?> GetUserProfileAsync(long id)
    {
        var profile = await _dbContext.UserProfiles
            .AsNoTracking()
            .Include(up => up.ImageFileInfo)
            .FirstOrDefaultAsync(up => up.Id == id);

        return profile?.ToContractUserProfile();
    }

    public async Task<List<Contract.UserProfile>> GetUserProfilesAsync(List<long> ids)
    {
        var profiles = await _dbContext.UserProfiles
            .AsNoTracking()
            .Include(up => up.ImageFileInfo)
            .Where(up => ids.Contains(up.Id))
            .ToListAsync();

        return profiles.Select(p => p.ToContractUserProfile()).ToList();
    }

    public async Task<Contract.UserProfile?> GetUserProfilesByIdentityGuidAsync(Guid guid)
    {
        var profile = await _dbContext.UserProfiles
            .AsNoTracking()
            .Include(up => up.ImageFileInfo)
            .FirstOrDefaultAsync(up => up.UserGuid == guid);

        return profile?.ToContractUserProfile();
    }


    public async Task<Contract.UserProfile> CreateUserProfileAsync(Contract.UserProfile newUserProfile)
    {
        var newDbProfile = new Entities.UserProfile
        {
            UserGuid = newUserProfile.UserIdentityGuid,
            RealName = newUserProfile.RealName,
            DateOfBirth = newUserProfile.DateOfBirth,
        };

        await _dbContext.UserProfiles.AddAsync(newDbProfile);
        await _dbContext.SaveChangesAsync();
        return newDbProfile.ToContractUserProfile();
    }


    public Task<Contract.UserProfile> UpdateUserProfileAsync(Contract.UserProfile newUserProfile)
    {
        throw new NotImplementedException();
    }
}