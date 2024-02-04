using DrugSchedule.Storage.Extensions;
using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserProfile = DrugSchedule.Storage.Data.Entities.UserProfile;
using DrugSchedule.StorageContract.Data;


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
        var profile = await _dbContext.UserProfiles
            .AsNoTracking()
            .Select(EntityMapExpressions.ToUserProfile(withAvatar))
            .FirstOrDefaultAsync(up => up.UserIdentityGuid == guid, cancellationToken);

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
        if (existingUserProfile is null)
        {
            return null;
        }

        if (updateFlags.RealName)
        {
            existingUserProfile.RealName = userProfile.RealName;
        }

        if (updateFlags.DateOfBirth)
        {
            existingUserProfile.DateOfBirth = userProfile.DateOfBirth;
        }

        if (updateFlags.AvatarGuid)
        {
            existingUserProfile.AvatarGuid = userProfile.Avatar?.Guid;
        }

        if (updateFlags.Sex)
        {
            existingUserProfile.Sex = userProfile.Sex;
        }

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? existingUserProfile.ToContractModel(false) : null;
    }

    public async Task<List<UserContact>> GetContactsAsync(long userProfileId, bool withAvatar, CancellationToken cancellationToken = default)
    {
        var contacts = await _dbContext.UserProfileContacts
            .AsNoTracking()
            .Include(c => c.ContactProfile)
            .Where(c => c.UserProfileId == userProfileId)
            .Select(EntityMapExpressions.ToUserContact(withAvatar))
            .ToListAsync(cancellationToken);

        return contacts;
    }

    public async Task<UserContact?> AddOrUpdateContactAsync(Contract.UserContact userContact, CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.UserProfileContacts
            .FirstOrDefaultAsync(c => c.UserProfileId == userContact.UserProfileId && c.ContactProfileId == userContact.Profile.UserProfileId, cancellationToken);

        contact ??= new Entities.UserProfileContact
        {
            UserProfileId = userContact.UserProfileId,
            ContactProfileId = userContact.Profile.UserProfileId,
            Name = userContact.CustomName
        };
        contact.Name = userContact.CustomName;

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? contact.ToContractModel(false) : null;
    }

    public async Task<bool> RemoveContactAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default)
    {
        var deleted = await _dbContext.UserProfileContacts
            .Where(c => c.UserProfileId == userProfileId && c.ContactProfileId == contactProfileId)
            .ExecuteDeleteAsync(cancellationToken);
        return deleted > 0;
    }

}