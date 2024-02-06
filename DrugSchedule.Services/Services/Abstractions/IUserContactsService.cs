using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Data;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IUserContactsService
{
    Task<OneOf<Models.UserContact, NotFound>> GetContactAsync(long contactProfileId, CancellationToken cancellationToken = default);

    Task<UserContactsCollection> GetContactsAsync(UserContactFilter filter, CancellationToken cancellationToken = default);

    Task<UserContactsSimpleCollection> GetContactsSimpleAsync(bool commonOnly, CancellationToken cancellationToken = default);

    Task<OneOf<True, InvalidInput, NotFound>> AddContactAsync(NewUserContact newContact, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveContactAsync(UserId userId, CancellationToken cancellationToken = default);
}