using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IUserContactsService
{
    Task<UserContactsCollectionModel> GetUserContactsAsync(CancellationToken cancellationToken = default);

    Task<OneOf<True, InvalidInput, NotFound>> AddUserContactAsync(NewUserContactModel newContactModel, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveUserContactAsync(UserIdModel userId, CancellationToken cancellationToken = default);
}