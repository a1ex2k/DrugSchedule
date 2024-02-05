using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class UserMedicamentExtendedCollection
{
    public required List<UserMedicamentExtendedModel> Medicaments { get; set; }
}