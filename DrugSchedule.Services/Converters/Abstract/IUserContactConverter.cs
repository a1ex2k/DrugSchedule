using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Converters;

public interface IUserContactConverter
{
    Models.UserContact ToContactExtended(StorageContract.Data.UserContact contact, UserIdentity identity);
    Models.UserContactSimple ToContactSimple(StorageContract.Data.UserContactSimple contact);
    UserContactsSimpleCollection ToContactSimpleCollection(List<StorageContract.Data.UserContactSimple> contacts);
}