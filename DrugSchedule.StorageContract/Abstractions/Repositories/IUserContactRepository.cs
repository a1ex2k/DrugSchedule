using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserContactRepository
{
    public Task<List<UserContact>> GetContactAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default);

    public Task<List<UserContact>> GetContactsAsync(long userProfileId, UserContactFilter filter, CancellationToken cancellationToken = default);

    public Task<List<UserContact>> GetContactsSimpleAsync(long userProfileId, CancellationToken cancellationToken = default);

    public Task<UserContactSimple?> AddOrUpdateContactAsync(long userProfileId, UserContactSimple userContact, CancellationToken cancellationToken = default);

    public Task<bool> RemoveContactAsync(long userProfileId, long contactProfileId, CancellationToken cancellationToken = default);
}