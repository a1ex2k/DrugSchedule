using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
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


    public Task<List<Contract.UserContact>> GetContactAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Contract.UserContact>> GetContactsAsync(long userProfileId, Contract.UserContactFilter filter, CancellationToken cancellationToken = default)
    {
        var contacts = await _dbContext.UserProfileContacts
            .AsNoTracking()
            .Where(c => c.UserProfileId == userProfileId)
            .WithFilter(c1 => c1.ContactProfileId, filter.ContactProfileIdFilter)
            .WithFilter(c2 => c2.Name, filter.ContactNameFilter)
            .Select(c => new Contract.UserContact
            {
                Profile = EntityMapExpressions.ToUserProfile(true).Compile().Invoke(c.ContactProfile!),
                CustomName = c.Name,
                IsCommon = _dbContext.UserProfileContacts
                    .Any(c2 => c2.UserProfileId == c.ContactProfileId
                               && c2.ContactProfileId == c.UserProfileId),
                HasSharedWith = c.SharedSchedules.Any(),
                HasSharedBy = _dbContext.ScheduleShare
                    .Any(s => s.MedicamentTakingSchedule!.UserProfileId == c.ContactProfileId
                              && s.ShareWithContactId == c.UserProfileId)

            })
            .ToListAsync(cancellationToken);

        return contacts;
    }

    public Task<List<Contract.UserContact>> GetContactsSimpleAsync(long userProfileId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }


    public async Task<Contract.UserContactSimple?> AddOrUpdateContactAsync(long userProfileId, Contract.UserContactSimple userContact, CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.UserProfileContacts
            .FirstOrDefaultAsync(c => c.UserProfileId == userProfileId
                                      && c.ContactProfileId == userContact.ContactProfileId, cancellationToken);

        contact ??= new Entities.UserProfileContact
        {
            UserProfileId = userProfileId,
            ContactProfileId = userContact.ContactProfileId,
            Name = userContact.CustomName
        };
        contact.Name = userContact.CustomName;
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);

        var isCommon = await _dbContext.UserProfileContacts
            .AnyAsync(c2 => c2.UserProfileId == contact.ContactProfileId
                            && c2.ContactProfileId == contact.UserProfileId, cancellationToken);

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