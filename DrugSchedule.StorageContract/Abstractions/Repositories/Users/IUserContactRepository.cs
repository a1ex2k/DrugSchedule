using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserContactRepository
{
    public Task<UserContact?> GetContactAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default);

    public Task<List<UserContact>> GetContactsAsync(long userProfileId, UserContactFilter filter, CancellationToken cancellationToken = default);

    public Task<UserContactSimple?> GetContactSimpleAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default);
    
    public Task<List<UserContactSimple>> GetContactsSimpleAsync(long userProfileId, bool commonOnly, CancellationToken cancellationToken = default);

    public Task<UserContactPlain?> AddOrUpdateContactAsync(UserContactPlain userContact, CancellationToken cancellationToken = default);

    public Task<bool> RemoveContactAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default);
}