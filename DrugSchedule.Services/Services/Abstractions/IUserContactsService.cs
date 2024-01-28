using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IUserContactsService
{
    Task<UserContactsCollection> GetUserContactsAsync(CancellationToken cancellationToken = default);

    Task<OneOf<True, InvalidInput, NotFound>> AddUserContactAsync(NewUserContact newContact, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveUserContactAsync(UserId userId, CancellationToken cancellationToken = default);
}