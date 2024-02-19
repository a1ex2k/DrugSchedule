using DrugSchedule.Services.Converters;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public class UserContactsService : IUserContactsService
{
    private readonly ICurrentUserIdentifier _currentUserIdentifier;
    private readonly IUserContactRepository _contactsRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IIdentityRepository _identityRepository;
    private readonly IUserContactConverter _converter;

    public UserContactsService(ICurrentUserIdentifier currentUserIdentifier, IFileService fileService, IUserContactRepository contactsRepository, IUserProfileRepository profileRepository, IIdentityRepository identityRepository, IUserContactConverter converter)
    {
        _currentUserIdentifier = currentUserIdentifier;
        _contactsRepository = contactsRepository;
        _profileRepository = profileRepository;
        _identityRepository = identityRepository;
        _converter = converter;
    }


    public async Task<OneOf<Models.UserContact, NotFound>> GetContactAsync(long contactProfileId, CancellationToken cancellationToken = default)
    {
        var contact = await _contactsRepository.GetContactAsync(_currentUserIdentifier.UserId, contactProfileId, cancellationToken);
        if (contact == null)
        {
            return new NotFound(ErrorMessages.NoContact);
        }

        var identity = await _identityRepository.GetUserIdentityAsync(contact.Profile.UserIdentityGuid, cancellationToken);
        var contactModel = _converter.ToContactExtended(contact, identity!);
        return contactModel;
    }


    public async Task<UserContactsCollection> GetContactsAsync(UserContactFilter filter, CancellationToken cancellationToken = default)
    {
        var contacts = await _contactsRepository.GetContactsAsync(_currentUserIdentifier.UserId, filter, cancellationToken);
        var identities = await _identityRepository.GetUserIdentitiesAsync(new UserIdentityFilter { GuidsFilter = contacts.Select(c => c.Profile.UserIdentityGuid).ToList() }, cancellationToken);

        var contactModelList = (
             from c in contacts
             join i in identities on c.Profile.UserIdentityGuid equals i.Guid
             select _converter.ToContactExtended(c, i)
                 ).ToList();
        return new UserContactsCollection
        {
            Contacts = contactModelList
        };
    }


    public async Task<UserContactsSimpleCollection> GetContactsSimpleAsync(bool commonOnly, CancellationToken cancellationToken = default)
    {
        var contacts = await _contactsRepository.GetContactsSimpleAsync(_currentUserIdentifier.UserId, commonOnly, cancellationToken);
        var collection = _converter.ToContactSimpleCollection(contacts);
        return collection;
    }


    public async Task<OneOf<True, InvalidInput, NotFound>> AddOrUpdateContactAsync(NewUserContact newContact, CancellationToken cancellationToken = default)
    {
        var userProfileExists = await _profileRepository.DoesUserProfileExistsAsync(newContact.UserProfileId, cancellationToken);
        if (!userProfileExists)
        {
            return new NotFound(ErrorMessages.UserNotFound);
        }

        var invalidInput = new InvalidInput();
        if (newContact.UserProfileId == _currentUserIdentifier.UserId)
        {
            invalidInput.Add($"Current user itself cannot be added to contacts");
        }

        if (string.IsNullOrWhiteSpace(newContact.СontactName))
        {
            invalidInput.Add($"Contact name must be non whitespace");
        }

        if (invalidInput.HasMessages) return invalidInput;

        var userContact = new UserContactPlain
        {
            ContactProfileId = newContact.UserProfileId,
            CustomName = newContact.СontactName,
            UserProfileId = _currentUserIdentifier.UserId,
        };

        _ = await _contactsRepository.AddOrUpdateContactAsync(userContact, cancellationToken);
        return new True();
    }


    public async Task<OneOf<True, NotFound>> RemoveContactAsync(long contactProfileId, CancellationToken cancellationToken = default)
    {
        var contactRemoved = await
            _contactsRepository.RemoveContactAsync(_currentUserIdentifier.UserId, contactProfileId, cancellationToken);
        if (!contactRemoved)
        {
            return new NotFound(ErrorMessages.NoContact);
        }

        return new True();
    }
}