using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class UserMedicamentExtendedCollection
{
    public required List<UserMedicamentExtendedModel> Medicaments { get; set; }
}