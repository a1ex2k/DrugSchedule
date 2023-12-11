using DrugSchedule.BusinessLogic.Auth;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using LoginResult = DrugSchedule.BusinessLogic.Utils.ServiceResult<(DrugSchedule.StorageContract.Data.UserIdentity Identity, DrugSchedule.StorageContract.Data.UserProfile Profile)>;

namespace DrugSchedule.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IIdentityRepository _identityRepository;
    private readonly IUserProfileRepository _profileRepository;

    public UserService(IIdentityRepository identityRepository, IUserProfileRepository profileRepository)
    {
        _identityRepository = identityRepository;
        _profileRepository = profileRepository;
    }


    public async Task<ServiceResult<UserIdentity>> RegisterUserAsync(RegisterModel registerModel)
    {
        var errors = new List<string>();

        if (CridentialsValidator.ValidatePassword(registerModel.Username))
        {
            errors.Add("Username must match the pattern: " + CridentialsValidator.UsernameRegexString);
        }

        if (string.IsNullOrWhiteSpace(registerModel.Email))
        {
            errors.Add("Email is empty");
        }

        if (CridentialsValidator.ValidatePassword(registerModel.Password))
        {
            errors.Add("Password must match the pattern: " + CridentialsValidator.PasswordRegexString);
        }

        if (errors.Count > 0)
        {
            return ServiceResult<UserIdentity>.Fail(errors);
        }

        var usernameUsed = await _identityRepository.IsUsernameUsedAsync(registerModel.Username!);
        var emailUsed = await _identityRepository.IsEmailUsedAsync(registerModel.Email!);

        if (usernameUsed || emailUsed)
        {
            errors.Add("Email or username is already used");
            return ServiceResult<UserIdentity>.Fail(errors);
        }

        var newUserIdentity = await _identityRepository.CreateUserIdentityAsync(new NewUserIdentity
        {
            Username = registerModel.Username!,
            Email = registerModel.Email!,
            Password = registerModel.Password!
        });

        return ServiceResult<UserIdentity>.Success(newUserIdentity);
    }


    public async Task<LoginResult> LogUserInAsync(LoginModel loginModel)
    {
        if (CridentialsValidator.ValidatePassword(loginModel.Username)
            || CridentialsValidator.ValidateUsername(loginModel.Password))
        {
            return LoginResult.Fail("Invalid username or password");
        }

        var userIdentity = await _identityRepository.GetUserIdentityAsync(loginModel.Username!);

        if (userIdentity == null)
        {
            return LoginResult.Fail("User does not exists");
        }

        var userProfile = await _profileRepository.GetUserProfilesByIdentityGuidAsync(userIdentity.Guid) 
            ?? await _profileRepository.CreateUserProfileAsync(new UserProfile
        {
            UserIdentityGuid = userIdentity.Guid,
            RealName = null,
            DateOfBirth = null,
        });

        return LoginResult.Success((userIdentity, userProfile));
    }
}