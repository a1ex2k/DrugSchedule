using System.Linq.Expressions;
using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class UserContactRepository : IUserContactRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<UserProfileRepository> _logger;

    public UserContactRepository(DrugScheduleContext dbContext, ILogger<UserProfileRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<UserContact?> GetContactAsync(long userProfileId, long contactProfileId,
        CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.UserProfileContacts
            .Where(c => c.UserProfileId == userProfileId && c.ContactProfileId == contactProfileId)
            .Select(ContactProjection)
            .FirstOrDefaultAsync(cancellationToken);

        return contact;
    }


    public async Task<List<UserContact>> GetContactsAsync(long userProfileId,
        UserContactFilter filter, CancellationToken cancellationToken = default)
    {
        var contacts = await _dbContext.UserProfileContacts
            .Where(c => c.UserProfileId == userProfileId)
            .WithFilter(c => c.ContactProfileId, filter.ContactProfileIdFilter)
            .WithFilter(c => c.CustomName, filter.ContactNameFilter)
            .Select(ContactProjection)
            .ToListAsync(cancellationToken);

        return contacts;
    }

    public async Task<UserContactSimple?> GetContactSimpleAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.UserProfileContacts
            .Where(c => c.UserProfileId == userProfileId)
            .Where(c => c.ContactProfileId == contactProfileId)
            .Select(EntityMapExpressions.ToContactSimple(_dbContext))
            .FirstOrDefaultAsync(cancellationToken);

        return contact;
    }


    public async Task<List<UserContactSimple>> GetContactsSimpleAsync(long userProfileId, bool commonOnly,
        CancellationToken cancellationToken = default)
    {
        var contactsQuery = _dbContext.UserProfileContacts
            .Where(c => c.UserProfileId == userProfileId)
            .Select(EntityMapExpressions.ToContactSimple(_dbContext))
            .WhereIf(commonOnly, c => c.IsCommon);

        var contacts = await contactsQuery.ToListAsync(cancellationToken);
        return contacts;
    }


    public async Task<UserContactPlain?> AddOrUpdateContactAsync(UserContactPlain userContact, CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.UserProfileContacts
            .FirstOrDefaultAsync(
                c => c.UserProfileId == userContact.UserProfileId && c.ContactProfileId == userContact.ContactProfileId,
                cancellationToken);

        if (contact == null)
        {
            contact = new Entities.UserProfileContact
            {
                UserProfileId = userContact.UserProfileId,
                ContactProfileId = userContact.ContactProfileId,
                CustomName = userContact.CustomName
            };
            await _dbContext.UserProfileContacts.AddAsync(contact, cancellationToken);
        }
        else
        {
            contact.CustomName = userContact.CustomName;
        }

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        if (!saved) return null;

        return new Contract.UserContactPlain
        {
            UserProfileId = contact.UserProfileId,
            ContactProfileId = contact.ContactProfileId,
            CustomName = contact.CustomName,
        };
    }


    public async Task<bool> RemoveContactAsync(long userProfileId, long contactProfileId,
        CancellationToken cancellationToken = default)
    {
        var deleted = await _dbContext.UserProfileContacts
            .Where(c => c.UserProfileId == userProfileId && c.ContactProfileId == contactProfileId)
            .ExecuteDeleteAsync(cancellationToken);
        return deleted > 0;
    }


    private Expression<Func<Entities.UserProfileContact, UserContact>> ContactProjection
        => (c) => new Contract.UserContact
        {
            Profile = EntityMapExpressions.ToUserProfile(true).Compile().Invoke(c.ContactProfile!),
            CustomName = c.CustomName,
            IsCommon = _dbContext.UserProfileContacts
                .Any(c2 => c2.UserProfileId == c.ContactProfileId
                           && c2.ContactProfileId == c.UserProfileId),
            HasSharedWith = c.ScheduleShares.Any(),
            HasSharedBy = _dbContext.ScheduleShare
                .Any(s => s.MedicamentTakingSchedule!.UserProfileId == c.ContactProfileId
                          && s.ShareWithContactId == c.UserProfileId),
        };
}