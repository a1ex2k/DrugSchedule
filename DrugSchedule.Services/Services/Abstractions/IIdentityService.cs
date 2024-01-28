using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IIdentityService
{
    Task<OneOf<NewUserIdentity, InvalidInput>> RegisterUserAsync(RegisterModel registerModel, CancellationToken cancellationToken = default);

    Task<OneOf<SuccessfulLogin, InvalidInput>> LogUserInAsync(LoginModel loginModel, CancellationToken cancellationToken = default);

    Task<AvailableUsername> IsUsernameAvailableAsync(string username, CancellationToken cancellationToken = default);
}