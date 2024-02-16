using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Data;
using UserContact = DrugSchedule.StorageContract.Data.UserContact;
using UserContactSimple = DrugSchedule.StorageContract.Data.UserContactSimple;

namespace DrugSchedule.Services.Converters;

public class UserContactConverter : IUserContactConverter
{
    private readonly IDownloadableFileConverter _downloadableFileConverter;

    public UserContactConverter(IDownloadableFileConverter downloadableFileConverter)
    {
        _downloadableFileConverter = downloadableFileConverter;
    }


    public Models.UserContact ToContactExtended(UserContact contact, UserIdentity identity)
        => new Models.UserContact
        {
            IsCommon = contact.IsCommon,
            HasSharedBy = contact.HasSharedBy,
            HasSharedWith = contact.HasSharedWith,
            UserProfileId = contact.Profile.UserProfileId,
            Username = identity!.Username!,
            СontactName = contact.CustomName,
            RealName = contact.Profile.RealName,
            Avatar = _downloadableFileConverter.ToFileModel(contact.Profile.Avatar,
            FileCategory.UserMedicamentImage.IsPublic()),
            DateOfBirth = contact.IsCommon ? contact.Profile.DateOfBirth : null,
            Sex = contact.IsCommon ? contact.Profile.Sex : null
        };


    public Models.UserContactSimple ToContactSimple(UserContactSimple contact)
        => new Models.UserContactSimple
        {
            UserProfileId = contact.ContactProfileId,
            СontactName = contact.CustomName,
            IsCommon = contact.IsCommon,
            ThumbnailUrl = _downloadableFileConverter.ToThumbLink(contact.Avatar, FileCategory.UserAvatar.IsPublic())
        };


    public Models.UserContactsSimpleCollection ToContactSimpleCollection(List<UserContactSimple> contacts)
        => new Models.UserContactsSimpleCollection
        {
            Contacts = contacts.ConvertAll(ToContactSimple)
        };
}