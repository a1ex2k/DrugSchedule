using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class UserMedicamentSimpleCollection
{
    public required List<UserMedicamentSimpleModel> Medicaments { get; set; }
}