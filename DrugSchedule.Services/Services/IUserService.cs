using DrugSchedule.BusinessLogic.Auth;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Services;

public interface IUserService
{
    Task<ServiceResult<UserIdentity>> RegisterUserAsync(RegisterModel registerModel);

    Task<ServiceResult<(UserIdentity Identity, UserProfile Profile)>> LogUserInAsync(LoginModel loginModel);
    
    Task<ServiceResult<AvailableUsernameModel>> IsUsernameAvailableAsync(string username);
}