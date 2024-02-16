using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Converters;

public interface IUserMedicamentConverter
{
    UserMedicamentUpdate ToUpdateResultModel(UserMedicament medicament);
    UserMedicamentExtendedModel ToUserMedicamentExtended(UserMedicamentExtended userMedicament, MedicamentExtended? globalMedicament);
    UserMedicamentSimpleModel ToUserMedicamentSimple(UserMedicamentSimple userMedicament);
    UserMedicamentSimpleCollection ToUserMedicamentSimpleCollection(List<UserMedicamentSimple> userMedicaments);
}