using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IIdentityService
{
    Task<OneOf<NewUserIdentityModel, InvalidInput>> RegisterUserAsync(RegisterModel registerModel, CancellationToken cancellationToken = default);

    Task<OneOf<SuccessfulLoginModel, InvalidInput>> LogUserInAsync(LoginModel loginModel, CancellationToken cancellationToken = default);

    Task<AvailableUsernameModel> IsUsernameAvailableAsync(string username, CancellationToken cancellationToken = default);
}