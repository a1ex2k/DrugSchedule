using DrugSchedule.Storage.Extensions;
using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserProfile = DrugSchedule.Storage.Data.Entities.UserProfile;
using System.Linq;


namespace DrugSchedule.Storage.Services;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<UserProfileRepository> _logger;

    public UserProfileRepository(DrugScheduleContext dbContext, ILogger<UserProfileRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Contract.UserProfile?> GetUserProfileAsync(long id, bool withAvatar, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.UserProfiles
            .AsNoTracking()
            .Select(EntityMapExpressions.ToUserProfile(withAvatar))
            .FirstOrDefaultAsync(up => up.UserProfileId == id, cancellationToken);
        return profile;
    }

    public async Task<bool> DoesUserProfileExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserProfiles
            .AnyAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<List<Contract.UserProfile>> GetUserProfilesAsync(List<long> ids, bool withAvatar, CancellationToken cancellationToken = default)
    {
        var profiles = await _dbContext.UserProfiles
            .AsNoTracking()
            .Where(up => ids.Contains(up.Id))
            .Select(EntityMapExpressions.ToUserProfile(withAvatar))
            .ToListAsync(cancellationToken);

        return profiles;
    }

    public async Task<Contract.UserProfile?> GetUserProfileAsync(Guid guid, bool withAvatar,
        CancellationToken cancellationToken = default)
    {
        var guidString = guid.ToString();
        var profile = await _dbContext.UserProfiles
            .AsNoTracking()
            .Where(u => u.IdentityGuid == guidString)
            .Select(EntityMapExpressions.ToUserProfile(withAvatar))
            .FirstOrDefaultAsync(cancellationToken);

        return profile;
    }

    public async Task<List<Contract.UserProfile>> GetUserProfilesAsync(List<Guid> guids, bool withAvatar, CancellationToken cancellationToken = default)
    {
        var guidStrings = guids.ConvertAll(g => g.ToString());
        var profiles = await _dbContext.UserProfiles
            .AsNoTracking()
            .Where(up => guidStrings.Contains(up.IdentityGuid))
            .Select(EntityMapExpressions.ToUserProfile(withAvatar))
            .ToListAsync(cancellationToken);

        return profiles;
    }


    public async Task<Contract.UserProfile?> CreateUserProfileAsync(Contract.UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        var newDbProfile = new UserProfile
        {
            IdentityGuid = userProfile.UserIdentityGuid.ToString(),
            RealName = userProfile.RealName,
            DateOfBirth = userProfile.DateOfBirth,
            AvatarGuid = userProfile.Avatar?.Guid,
            Sex = userProfile.Sex
        };

        await _dbContext.UserProfiles.AddAsync(newDbProfile, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? newDbProfile.ToContractModel(false) : null;
    }

    public async Task<Contract.UserProfile?> UpdateUserProfileAsync(Contract.UserProfile userProfile, Contract.UserProfileUpdateFlags updateFlags,
        CancellationToken cancellationToken = default)
    {
        var existingUserProfile = await _dbContext.UserProfiles
            .FirstOrDefaultAsync(up => up.Id == userProfile.UserProfileId, cancellationToken);
        
        if (existingUserProfile is null) return null;

        var entry = _dbContext.Entry(existingUserProfile);
        entry.UpdateIf(e => e.RealName, userProfile.RealName, updateFlags.RealName);
        entry.UpdateIf(e => e.DateOfBirth, userProfile.DateOfBirth, updateFlags.DateOfBirth);
        entry.UpdateIf(e => e.Sex, userProfile.Sex, updateFlags.Sex);
        entry.UpdateIf(e => e.AvatarGuid, userProfile.Avatar?.Guid, updateFlags.AvatarGuid);

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? existingUserProfile.ToContractModel(false) : null;
    }

}