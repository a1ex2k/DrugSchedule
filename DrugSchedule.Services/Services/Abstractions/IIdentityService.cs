using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;

namespace DrugSchedule.Services.Services.Abstractions;

public interface IIdentityService
{
    Task<OneOf<NewUserIdentity, InvalidInput>> RegisterUserAsync(RegisterModel registerModel, CancellationToken cancellationToken = default);

    Task<OneOf<SuccessfulLogin, InvalidInput>> LogUserInAsync(LoginModel loginModel, CancellationToken cancellationToken = default);

    Task<AvailableUsername> IsUsernameAvailableAsync(string username, CancellationToken cancellationToken = default);
}