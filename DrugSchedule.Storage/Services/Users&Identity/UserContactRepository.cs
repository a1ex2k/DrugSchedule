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
            .Select(EntityMapExpressions.ToContact(_dbContext))
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
            .Select(EntityMapExpressions.ToContact(_dbContext))
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
            .WhereIf(commonOnly, c => _dbContext.UserProfileContacts
                .Any(c2 => c2.UserProfileId == c.ContactProfileId
                           && c2.ContactProfileId == c.UserProfileId))
            .Select(EntityMapExpressions.ToContactSimple(_dbContext));

        var contacts = await contactsQuery.ToListAsync(cancellationToken);
        return contacts;
    }


    public async Task<bool?> IsContactCommon(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default)
    {
        var dbResult = await _dbContext.UserProfileContacts
            .Where(c => c.UserProfileId == userProfileId)
            .Where(c => c.ContactProfileId == contactProfileId)
            .Select(c => new
            { 
                IsCommon = c.ContactProfile!.Contacts
                    .AsQueryable()
                    .Any(c2 => c2.ContactProfileId  == userProfileId)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return dbResult?.IsCommon;
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


}