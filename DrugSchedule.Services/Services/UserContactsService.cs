using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Services.Abstractions;
using DrugSchedule.BusinessLogic.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;

using OneOf.Types;
using UserContact = DrugSchedule.BusinessLogic.Models.UserContact;

namespace DrugSchedule.BusinessLogic.Services;

public class UserContactsService : IUserContactsService
{
    private readonly IDownloadableFileConverter _downloadableFileConverter;
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly IUserContactRepository _contactsRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IIdentityRepository _identityRepository;

    public UserContactsService(ICurrentUserIdentifier currentUserIdentifier, IFileService fileService, IDownloadableFileConverter downloadableFileConverter, IUserContactRepository contactsRepository, IUserProfileRepository profileRepository, IIdentityRepository identityRepository)
    {
        _currentUserIdentifier = currentUserIdentifier;
        _downloadableFileConverter = downloadableFileConverter;
        _contactsRepository = contactsRepository;
        _profileRepository = profileRepository;
        _identityRepository = identityRepository;
    }


    public async Task<OneOf<UserContact, NotFound>> GetContactAsync(long contactProfileId, CancellationToken cancellationToken = default)
    {
        var contact = await _contactsRepository.GetContactAsync(_currentUserIdentifier.UserProfileId, contactProfileId, cancellationToken);
        if (contact == null)
        {
            return new NotFound("No contact with such ID found");
        }

        var identity = await _identityRepository.GetUserIdentityAsync(contact.Profile.UserIdentityGuid, cancellationToken);
        var contactModel = BuildContactModel(contact, identity!);
        return contactModel;
    }


    public async Task<UserContactsCollection> GetContactsAsync(UserContactFilter filter, CancellationToken cancellationToken = default)
    {
        var contacts = await _contactsRepository.GetContactsAsync(_currentUserIdentifier.UserProfileId, filter, cancellationToken);
        var identities = await _identityRepository.GetUserIdentitiesAsync(new UserIdentityFilter { GuidsFilter = contacts.Select(c => c.Profile.UserIdentityGuid).ToList() }, cancellationToken);

        var contactModelList = (
             from c in contacts
             join i in identities on c.Profile.UserIdentityGuid equals i.Guid
             select BuildContactModel(c, i)
                 ).ToList();
        return new UserContactsCollection
        {
            Contacts = contactModelList
        };
    }

    public async Task<UserContactsSimpleCollection> GetContactsSimpleAsync(bool commonOnly, CancellationToken cancellationToken = default)
    {
        var contacts = await _contactsRepository.GetContactsSimpleAsync(_currentUserIdentifier.UserProfileId, commonOnly, cancellationToken);
        var model = new UserContactsSimpleCollection
        {
            Contacts = contacts.ConvertAll(c => new Models.UserContactSimple
            {
                UserProfileId = c.ContactProfileId,
                СontactName = c.CustomName,
                IsCommon = c.IsCommon,
                Avatar = c.Avatar == null ? null : _downloadableFileConverter.ToDownloadableFile(c.Avatar, true)
            })
        };
        return model;
    }


    public async Task<OneOf<True, InvalidInput, NotFound>> AddContactAsync(NewUserContact newContact, CancellationToken cancellationToken = default)
    {
        var userProfileExists = await _profileRepository.DoesUserProfileExistsAsync(newContact.UserProfileId, cancellationToken);
        if (!userProfileExists)
        {
            return new NotFound($"User with Id={newContact.UserProfileId} not found");
        }

        var invalidInput = new InvalidInput();
        if (newContact.UserProfileId == _currentUserIdentifier.UserProfileId)
        {
            invalidInput.Add($"Current user itself cannot be added to contacts");
        }

        if (string.IsNullOrWhiteSpace(newContact.СontactName))
        {
            invalidInput.Add($"Contact name must be non whitespace");
        }

        if (invalidInput.HasMessages) return invalidInput;

        var userContact = new StorageContract.Data.UserContactSimple
        {
            ContactProfileId = newContact.UserProfileId,
            CustomName = newContact.СontactName,
        };

        var addedContact = await _contactsRepository.AddOrUpdateContactAsync(_currentUserIdentifier.UserProfileId, userContact, cancellationToken);
        return new True();
    }


    public async Task<OneOf<True, NotFound>> RemoveContactAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var contactRemoved = await
            _contactsRepository.RemoveContactAsync(_currentUserIdentifier.UserProfileId, userId.UserProfileId, cancellationToken);
        if (!contactRemoved)
        {
            return new NotFound("No contact with such ID found");
        }

        return new True();
    }


    private Models.UserContact BuildContactModel(StorageContract.Data.UserContact contact, UserIdentity identity) => new UserContact
    {
        IsCommon = contact.IsCommon,
        HasSharedBy = contact.HasSharedBy,
        HasSharedWith = contact.HasSharedWith,
        UserProfileId = contact.Profile.UserProfileId,
        Username = identity!.Username!,
        СontactName = contact.CustomName,
        RealName = contact.Profile.RealName,
        Avatar = contact.Profile.Avatar is null
            ? null
            : _downloadableFileConverter.ToDownloadableFile(contact.Profile.Avatar,
                true),
        DateOfBirth = contact.IsCommon ? contact.Profile.DateOfBirth : null,
        Sex = contact.IsCommon ? contact.Profile.Sex : null
    };
}