using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;
using OneOf.Types;

namespace DrugSchedule.Services.Services.Abstractions;

public interface IUserContactsService
{
    Task<OneOf<Models.UserContact, NotFound>> GetContactAsync(long contactProfileId, CancellationToken cancellationToken = default);

    Task<UserContactsCollection> GetContactsAsync(UserContactFilter filter, CancellationToken cancellationToken = default);

    Task<UserContactsSimpleCollection> GetContactsSimpleAsync(bool commonOnly, CancellationToken cancellationToken = default);

    Task<OneOf<True, InvalidInput, NotFound>> AddContactAsync(NewUserContact newContact, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveContactAsync(long contactProfileId, CancellationToken cancellationToken = default);
}