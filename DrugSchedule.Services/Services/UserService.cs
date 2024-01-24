using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Services.Abstractions;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.UserStorage;
using OneOf.Types;

namespace DrugSchedule.BusinessLogic.Services;

public class UserService : IIdentityService, ICurrentUserService
{
    private readonly IFileInfoService _fileInfoService;
    private readonly IIdentityRepository _identityRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly ICurrentUserIdentificator _currentUserIdentificator;

    public UserService(IIdentityRepository identityRepository, IUserProfileRepository profileRepository, ICurrentUserIdentificator currentUserIdentificator, IFileInfoService fileInfoService)
    {
        _identityRepository = identityRepository;
        _profileRepository = profileRepository;
        _currentUserIdentificator = currentUserIdentificator;
        _fileInfoService = fileInfoService;
    }


    public async Task<OneOf<NewUserIdentityModel, InvalidInput>> RegisterUserAsync(RegisterModel registerModel, CancellationToken cancellationToken = default)
    {
        var error = new InvalidInput();

        if (!CridentialsValidator.ValidateUsername(registerModel.Username))
        {
            error.Add("Username must match the pattern: " + CridentialsValidator.UsernameRegexString);
        }

        if (string.IsNullOrWhiteSpace(registerModel.Email))
        {
            error.Add("Email is invalid");
        }

        if (!CridentialsValidator.ValidatePassword(registerModel.Password))
        {
            error.Add("Password must match the pattern: " + CridentialsValidator.PasswordRegexString);
        }

        if (string.Equals(registerModel.Password, registerModel.Username, StringComparison.OrdinalIgnoreCase))
        {
            error.Add("Password and username must differ");
        }

        if (error.HasMessages)
        {
            return error;
        }

        var usernameUsed = await _identityRepository.IsUsernameUsedAsync(registerModel.Username!, cancellationToken);
        var emailUsed = await _identityRepository.IsEmailUsedAsync(registerModel.Email!, cancellationToken);

        if (usernameUsed || emailUsed)
        {
            error.Add("Email or username is already used");
            return error;
        }

        var newUserIdentity = await _identityRepository.CreateUserIdentityAsync(new NewUserIdentity
        {
            Username = registerModel.Username!,
            Email = registerModel.Email!,
            Password = registerModel.Password!
        }, cancellationToken);

        return new NewUserIdentityModel
        {
            Username = newUserIdentity.Username!,
            Email = newUserIdentity.Email!
        };
    }


    public async Task<OneOf<SuccessfulLoginModel, InvalidInput>> LogUserInAsync(LoginModel loginModel, CancellationToken cancellationToken = default)
    {
        var userIdentity = await _identityRepository.GetUserIdentityAsync(loginModel.Username!, loginModel.Password!, cancellationToken);
        if (userIdentity == null)
        {
            return new InvalidInput("Either username or password is incorrect");
        }

        var userProfile = await _profileRepository.GetUserProfileAsync(userIdentity.Guid, cancellationToken) 
                          ?? await _profileRepository.CreateUserProfileAsync(new UserProfile
        {
            UserIdentityGuid = userIdentity.Guid,
            RealName = null,
            DateOfBirth = null,
            Sex = Sex.Undefined,
            AvatarGuid = null,
        }, cancellationToken);

        var successfulLoginModel = new SuccessfulLoginModel
        {
            UserProfileId = userProfile.UserProfileId,
            UserIdentityGuid = userIdentity.Guid,
            Username = userIdentity.Username!,
            Email = userIdentity.Email!
        };

        return successfulLoginModel;
    }


    public async Task<AvailableUsernameModel> IsUsernameAvailableAsync(string username, CancellationToken cancellationToken = default)
    {
        if (!CridentialsValidator.ValidateUsername(username))
        {
            return new AvailableUsernameModel
            {
                Username = username,
                IsAvailable = false,
                Comment = "Username doesn't match the pattern: " + CridentialsValidator.UsernameRegexString
            };
        }

        var isUsed = await _identityRepository.IsUsernameUsedAsync(username, cancellationToken);
        return new AvailableUsernameModel
        {
            Username = username,
            IsAvailable = !isUsed,
            Comment = isUsed ? "Username already used" : "Free"
        };
    }


    public async Task<OneOf<True, InvalidInput>> UpdatePasswordAsync(NewPasswordModel newPassword, CancellationToken cancellationToken = default)
    {
        var identity = await _identityRepository.GetUserIdentityAsync(_currentUserIdentificator.UserIdentityGuid, cancellationToken);

        var error = new InvalidInput();
        if (!CridentialsValidator.ValidatePassword(newPassword.NewPassword))
        {
            error.Add("Password must match the pattern: " + CridentialsValidator.PasswordRegexString);
        }

        if (string.Equals(newPassword.NewPassword, identity.Username, StringComparison.OrdinalIgnoreCase))
        {
            error.Add("Password and username must differ");
        }

        if (error.HasMessages)
        {
            return error;
        }

        var passwordUpdate = new PasswordUpdate
        {
            IdentityGuid = _currentUserIdentificator.UserIdentityGuid,
            OldPassword = newPassword.OldPassword,
            NewPassword = newPassword.NewPassword
        };
        var passwordWasUpdated = await _identityRepository.UpdatePasswordAsync(passwordUpdate, cancellationToken);

        if (!passwordWasUpdated)
        {
            error.Add("Old password doesn't match or same to new one");
            return error;
        }

        return new True();
    }


    public async Task<OneOf<UserModel, InvalidInput>> UpdateProfileAsync(UserUpdateModel userUpdateModel, CancellationToken cancellationToken = default)
    {
        var updateFlags = new UserProfileUpdateFlags
        {
            RealName = userUpdateModel.RealName != null,
            DateOfBirth = userUpdateModel.DateOfBirth.HasValue,
            Sex = userUpdateModel.Sex.HasValue,
        };

        var error = new InvalidInput();

        if (updateFlags.DateOfBirth 
            && userUpdateModel.DateOfBirth!.Value != DateOnly.MinValue)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            if (userUpdateModel.DateOfBirth.Value > currentDate.AddYears(-5)
                || userUpdateModel.DateOfBirth.Value < currentDate.AddYears(-120))
            {
                error.Add("Invalid DateOfBirth: 01.01.0001 to reset, current age > 5 and < 120");
            }
        }

        if (updateFlags.RealName && userUpdateModel.RealName!.Length > 20)
        {
            error.Add("Invalid RealName: empty string to reset, up to 20 characters");
        }

        var userProfile = new UserProfile
        {
            UserProfileId = _currentUserIdentificator.UserProfileId,
            RealName = string.IsNullOrWhiteSpace(userUpdateModel.RealName) ? null : userUpdateModel.RealName.Trim(),
            DateOfBirth = userUpdateModel.DateOfBirth == DateOnly.MinValue ? null : userUpdateModel.DateOfBirth,
            UserIdentityGuid = _currentUserIdentificator.UserIdentityGuid,
            Sex = userUpdateModel.Sex ?? Sex.Undefined,
        };
        var updateResult = await _profileRepository.UpdateUserProfileAsync(userProfile, updateFlags, cancellationToken);

        var identity = await _identityRepository.GetUserIdentityAsync(_currentUserIdentificator.UserIdentityGuid, cancellationToken);
        var userModel = await GetUserModelAsync(identity!, updateResult, false, cancellationToken);
        return userModel!;
    }


    public async Task<UserModel> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var userIdentity = await _identityRepository.GetUserIdentityAsync(_currentUserIdentificator.UserIdentityGuid, cancellationToken);
        var userModel = await GetUserModelAsync(userIdentity, null, false, cancellationToken);
        return userModel;
    }


    public async Task<UserContactsCollectionModel> GetUserContactsAsync(CancellationToken cancellationToken = default)
    {
        var contacts = await _profileRepository.GetContactsAsync(_currentUserIdentificator.UserProfileId, cancellationToken);
        var identities = await _identityRepository.GetUserIdentitiesAsync(contacts.ConvertAll(c => c.Profile.UserIdentityGuid), cancellationToken);
        var avatarInfos = await _fileInfoService.GetFileInfosAsync(contacts
            .Where(c => c.Profile.AvatarGuid.HasValue)
            .Select(c => c.Profile.AvatarGuid!.Value).ToList(), cancellationToken);

        var contactModelList = contacts
            .Join(identities, c => c.Profile.UserIdentityGuid, i => i.Guid, (c, i) =>
                new UserContactModel
                {
                    Id = c.Profile.UserProfileId,
                    Username = i.Username!,
                    СontactName = c.CustomName,
                    RealName = c.Profile.RealName,
                    Avatar = c.Profile.AvatarGuid == null
                        ? null
                        : avatarInfos.Find(f => f.Guid == c.Profile.AvatarGuid.Value)?.ToFileInfoModel()
                })
            .ToList();
        return new UserContactsCollectionModel
        {
            UserId = _currentUserIdentificator.UserProfileId,
            Contacts = contactModelList
        };
    }


    public async Task<OneOf<UserPublicCollectionModel, InvalidInput>> FindUsersAsync(UserSearchModel searchModel, CancellationToken cancellationToken = default)
    {
        var usernamePart = searchModel.UsernameSubstring;
        if (string.IsNullOrWhiteSpace(usernamePart.Trim()) || usernamePart.Length < 3)
        {
            return new InvalidInput("Search value must be at least 3 not whitespace characters long");
        }
        
        var identities = await _identityRepository.GetUserIdentitiesAsync(usernamePart, cancellationToken);
        var profiles = await _profileRepository.GetUserProfilesAsync(identities.ConvertAll(i => i.Guid), cancellationToken);
        var avatarInfos = await _fileInfoService.GetFileInfosAsync(profiles
            .Where(p => p.AvatarGuid.HasValue)
            .Select(p => p.AvatarGuid!.Value).ToList(), cancellationToken);

        var usersList = identities
            .Where(idn => idn.Guid != _currentUserIdentificator.UserIdentityGuid)
            .Join(profiles, i => i.Guid, p => p.UserIdentityGuid, (i, p) =>
                new PublicUserModel()
                {
                    Id = p.UserProfileId,
                    Username = i.Username!,
                    RealName = p.RealName,
                    Avatar = p.AvatarGuid == null
                        ? null
                        : avatarInfos.Find(f => f.Guid == p.AvatarGuid.Value)?.ToFileInfoModel()
                })
            .ToList();
        return new UserPublicCollectionModel
        {
            Users = usersList
        };
    }


    public async Task<OneOf<True, InvalidInput, NotFound>> AddUserContactAsync(NewUserContactModel newContactModel, CancellationToken cancellationToken = default)
    {
        if (newContactModel.UserProfileId == _currentUserIdentificator.UserProfileId)
        {
            return new InvalidInput($"Current user itself cannot be added to contacts");
        }

        var userProfile = await _profileRepository.GetUserProfileAsync(newContactModel.UserProfileId, cancellationToken);
        if (userProfile == null)
        {
            return new NotFound($"User with Id={newContactModel.UserProfileId} not found");
        }

        var customName = newContactModel.CustomName;
        if (string.IsNullOrWhiteSpace(customName))
        {
            customName = userProfile.RealName;
        }

        if (string.IsNullOrWhiteSpace(customName))
        {
            var identity = await _identityRepository.GetUserIdentityAsync(userProfile.UserIdentityGuid, cancellationToken);
            customName = identity!.Username;
        }

        var userContact = new UserContact
        {
            UserProfileId = _currentUserIdentificator.UserProfileId,
            Profile = userProfile,
            CustomName = customName ?? "user" + userProfile.UserProfileId,
        };
        var addedContact = await _profileRepository.AddOrUpdateContactAsync(userContact, cancellationToken);
        return new True();
    }


    public async Task<OneOf<True, NotFound>> RemoveUserContactAsync(UserIdModel userId, CancellationToken cancellationToken = default)
    {
        var contactRemoved = await
            _profileRepository.RemoveContactAsync(_currentUserIdentificator.UserProfileId, userId.UserProfileId, cancellationToken);
        if (!contactRemoved)
        {
            return new NotFound("No contact with such ID found");
        }

        return new True();
    }


    public async Task<OneOf<FileInfoModel, InvalidInput>> SetAvatarAsync(NewFileModel newAvatar, CancellationToken cancellationToken = default)
    {
        var newFile = new NewFile
        {
            NameWithExtension = newAvatar.NameWithExtension,
            Category = FileCategory.UserAvatar,
            MediaType = newAvatar.MediaType,
            Stream = newAvatar.Stream,
        };
        var fileServiceResult = await _fileInfoService.CreateAsync(newFile, cancellationToken);

        if (fileServiceResult.IsT1)
        {
            return fileServiceResult.AsT1;
        }

        var profile = new UserProfile
        {
            UserProfileId = _currentUserIdentificator.UserProfileId,
            AvatarGuid = fileServiceResult.AsT0.Guid
        };
        var updateFlags = new UserProfileUpdateFlags
        {
            AvatarGuid = true
        };
        var updateResult = await _profileRepository.UpdateUserProfileAsync(profile, updateFlags, cancellationToken);

        return fileServiceResult.AsT0.ToFileInfoModel();
    }


    public async Task<OneOf<True, NotFound>> RemoveAvatarAsync(FileInfoRemoveModel fileInfoRemoveModel, CancellationToken cancellationToken = default)
    {
        var profile = new UserProfile
        {
            UserProfileId = _currentUserIdentificator.UserProfileId,
            UserIdentityGuid = _currentUserIdentificator.UserIdentityGuid,
            AvatarGuid = null
        };
        var updateFlags = new UserProfileUpdateFlags
        {
            AvatarGuid = true
        };
        var updateResult = await _profileRepository.UpdateUserProfileAsync(profile, updateFlags, cancellationToken);
        if (updateResult == null)
        {
            return new NotFound("Cannot remove avatar");
        }

        return new True();
    }

    public async Task<OneOf<True, NotFound>> GetAvatarsInfoAsync(FileInfoRequestModel fileInfoRemoveModel, CancellationToken cancellationToken = default)
    {
        var avatarsInfo =  
    }


    private async Task<UserModel?> GetUserModelAsync(UserIdentity userIdentity, UserProfile? userProfile, bool createIfNotExists, CancellationToken cancellationToken)
    {
        userProfile ??= await _profileRepository.GetUserProfileAsync(userIdentity.Guid, cancellationToken);
        if (userProfile == null)
        {
            if (!createIfNotExists)
            {
                return null;
            }

            userProfile = await _profileRepository.CreateUserProfileAsync(new UserProfile
            {
                UserIdentityGuid = userIdentity.Guid,
                RealName = null,
                DateOfBirth = null,
                Sex = Sex.Undefined,
                AvatarGuid = null,
            }, cancellationToken);
        }

        FileInfo avatar = null;
        if (userProfile.AvatarGuid != null)
        {
            var avatarResult = await _fileInfoService.GetFileInfoAsync(userProfile.AvatarGuid.Value, cancellationToken);
            avatar = avatarResult.AsT0;
        }
        var userModel = new UserModel
        {
            Id = userProfile.UserProfileId,
            Username = userIdentity.Username!,
            Email = userIdentity.Email!,
            RealName = userProfile.RealName,
            DateOfBirth = userProfile.DateOfBirth,
            Sex = userProfile.Sex,
            Avatar = avatar?.ToFileInfoModel()
        };

        return userModel;
    }
}