using DrugSchedule.Services.Converters;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.UserStorage;
using OneOf.Types;
using NewUserIdentity = DrugSchedule.Services.Models.NewUserIdentity;

namespace DrugSchedule.Services.Services;

public class UserService : IIdentityService, IUserService
{
    private readonly IFileService _fileService;
    private readonly IDownloadableFileConverter _downloadableFileConverter;
    private readonly IIdentityRepository _identityRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly ICurrentUserIdentifier _currentUserIdentifier;

    public UserService(IIdentityRepository identityRepository, IUserProfileRepository profileRepository, ICurrentUserIdentifier currentUserIdentifier, IFileService fileService, IDownloadableFileConverter downloadableFileConverter)
    {
        _identityRepository = identityRepository;
        _profileRepository = profileRepository;
        _currentUserIdentifier = currentUserIdentifier;
        _fileService = fileService;
        _downloadableFileConverter = downloadableFileConverter;
    }


    public async Task<OneOf<NewUserIdentity, InvalidInput>> RegisterUserAsync(RegisterModel registerModel, CancellationToken cancellationToken = default)
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

        var newUserIdentity = await _identityRepository.CreateUserIdentityAsync(new StorageContract.Data.NewUserIdentity
        {
            Username = registerModel.Username!,
            Email = registerModel.Email!,
            Password = registerModel.Password!
        }, cancellationToken);

        return new NewUserIdentity
        {
            Username = newUserIdentity.Username!,
            Email = newUserIdentity.Email!
        };
    }


    public async Task<OneOf<SuccessfulLogin, InvalidInput>> LogUserInAsync(LoginModel loginModel, CancellationToken cancellationToken = default)
    {
        var userIdentity = await _identityRepository.GetUserIdentityAsync(loginModel.Username!, loginModel.Password!, cancellationToken);
        if (userIdentity == null)
        {
            return new InvalidInput("Either username or password is incorrect");
        }

        var userProfile = await _profileRepository.GetUserProfileAsync(userIdentity.Guid, false, cancellationToken)
                          ?? await _profileRepository.CreateUserProfileAsync(new UserProfile
                          {
                              UserIdentityGuid = userIdentity.Guid,
                              RealName = null,
                              DateOfBirth = null,
                              Sex = Sex.Undefined,
                          }, cancellationToken);

        var successfulLoginModel = new SuccessfulLogin
        {
            UserProfileId = userProfile!.UserProfileId,
            UserIdentityGuid = userIdentity.Guid,
            Username = userIdentity.Username!,
            Email = userIdentity.Email!
        };

        return successfulLoginModel;
    }


    public async Task<AvailableUsername> IsUsernameAvailableAsync(string username, CancellationToken cancellationToken = default)
    {
        if (!CridentialsValidator.ValidateUsername(username))
        {
            return new AvailableUsername
            {
                Username = username,
                IsAvailable = false,
                Comment = "Username doesn't match the pattern: " + CridentialsValidator.UsernameRegexString
            };
        }

        var isUsed = await _identityRepository.IsUsernameUsedAsync(username, cancellationToken);
        return new AvailableUsername
        {
            Username = username,
            IsAvailable = !isUsed,
            Comment = isUsed ? "Username already used" : "Free"
        };
    }


    public async Task<OneOf<True, InvalidInput>> UpdatePasswordAsync(NewPasswordModel newPassword, CancellationToken cancellationToken = default)
    {
        var identity = await _identityRepository.GetUserIdentityAsync(_currentUserIdentifier.UserIdentityGuid, cancellationToken);

        var error = new InvalidInput();
        if (!CridentialsValidator.ValidatePassword(newPassword.NewPassword))
        {
            error.Add("Password must match the pattern: " + CridentialsValidator.PasswordRegexString);
        }

        if (string.Equals(newPassword.NewPassword, identity!.Username, StringComparison.OrdinalIgnoreCase))
        {
            error.Add("Password and username must differ");
        }

        if (error.HasMessages)
        {
            return error;
        }

        var passwordUpdate = new PasswordUpdate
        {
            IdentityGuid = _currentUserIdentifier.UserIdentityGuid,
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


    public async Task<OneOf<True, InvalidInput>> UpdateProfileAsync(UserUpdate userUpdate, CancellationToken cancellationToken = default)
    {
        var updateFlags = new UserProfileUpdateFlags
        {
            RealName = true,
            DateOfBirth = true,
            Sex = true,
        };

        var error = new InvalidInput();

        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (userUpdate.DateOfBirth > currentDate.AddYears(-5)
            || userUpdate.DateOfBirth < currentDate.AddYears(-120))
        {
            error.Add("Invalid date of birth. Current age must be greater than 5 and less than 120");
        }

        if (userUpdate.RealName != null && userUpdate.RealName.Trim().Length > 30)
        {
            error.Add("Invalid real name. Max 30 characters");
        }

        if (error.HasMessages) return error;

        var userProfile = new UserProfile
        {
            UserProfileId = _currentUserIdentifier.UserProfileId,
            RealName = userUpdate.RealName?.Trim(),
            DateOfBirth = userUpdate.DateOfBirth,
            UserIdentityGuid = _currentUserIdentifier.UserIdentityGuid,
            Sex = userUpdate.Sex,
        };

        _ = await _profileRepository.UpdateUserProfileAsync(userProfile, updateFlags, cancellationToken);
        return new True();
    }


    public async Task<UserFull> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var userIdentity = await _identityRepository.GetUserIdentityAsync(_currentUserIdentifier.UserIdentityGuid, cancellationToken);
        var userProfile = await _profileRepository.GetUserProfileAsync(userIdentity!.Guid, true, cancellationToken);

        var userModel = new UserFull
        {
            Id = userProfile!.UserProfileId,
            Username = userIdentity.Username!,
            Email = userIdentity.Email!,
            RealName = userProfile.RealName,
            DateOfBirth = userProfile.DateOfBirth,
            Sex = userProfile.Sex,
            Avatar = _downloadableFileConverter.ToFileModel(userProfile.Avatar, FileCategory.UserAvatar.IsPublic())
        };

        return userModel;
    }


    public async Task<OneOf<UserPublicCollection, InvalidInput>> FindUsersAsync(UserSearch search, CancellationToken cancellationToken = default)
    {
        var usernamePart = search.UsernameSubstring;
        if (string.IsNullOrWhiteSpace(usernamePart.Trim()) || usernamePart.Length < 3)
        {
            return new InvalidInput("Search value must be at least 3 not whitespace characters long");
        }

        var filter = new UserIdentityFilter
        {
            UsernameFilter = new StringFilter
            {
                SubString = search.UsernameSubstring,
                StringSearchType = StringSearch.Contains
            },
            Take = search.MaxCount
        };

        var identities = await _identityRepository.GetUserIdentitiesAsync(filter, cancellationToken);
        var profiles = await _profileRepository.GetUserProfilesAsync(identities.ConvertAll(i => i.Guid), true, cancellationToken);

        var usersList = (
                from p in profiles
                join i in identities on p.UserIdentityGuid equals i.Guid
                select new PublicUser()
                {
                    Id = p.UserProfileId,
                    Username = i.Username!,
                    RealName = p.RealName,
                    ThumbnailUrl = _downloadableFileConverter.ToThumbLink(p.Avatar, FileCategory.UserAvatar.IsPublic())
                })
            .ToList();
        return new UserPublicCollection
        {
            Users = usersList
        };
    }


    public async Task<OneOf<DownloadableFile, InvalidInput>> SetAvatarAsync(InputFile inputAvatar, CancellationToken cancellationToken = default)
    {
        var fileServiceResult = await _fileService.CreateAsync(inputAvatar,
            FileCategory.UserAvatar.GetAwaitableParams(),
            FileCategory.UserAvatar,
            cancellationToken);

        if (fileServiceResult.IsT1)
        {
            return fileServiceResult.AsT1;
        }

        var profile = new UserProfile
        {
            UserProfileId = _currentUserIdentifier.UserProfileId,
            Avatar = fileServiceResult.AsT0
        };
        var updateFlags = new UserProfileUpdateFlags
        {
            AvatarGuid = true
        };
        var updateResult = await _profileRepository.UpdateUserProfileAsync(profile, updateFlags, cancellationToken);
        var file = _downloadableFileConverter.ToFileModel(fileServiceResult.AsT0, FileCategory.UserAvatar.IsPublic())!;
        return file;
    }


    public async Task<OneOf<True, NotFound>> RemoveAvatarAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        var user = await _profileRepository.GetUserProfileAsync(_currentUserIdentifier.UserProfileId, true, cancellationToken);
        if (user?.Avatar?.Guid != fileGuid)
        {
            return new NotFound("Avatar with such Guid not found avatar");
        }

        var profile = new UserProfile
        {
            UserProfileId = _currentUserIdentifier.UserProfileId,
            UserIdentityGuid = _currentUserIdentifier.UserIdentityGuid,
            Avatar = null
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

        var removeResult = await _fileService.RemoveFileAsync(fileGuid, cancellationToken);
        return new True();
    }

}