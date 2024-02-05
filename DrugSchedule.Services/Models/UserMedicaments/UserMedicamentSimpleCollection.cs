using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class UserMedicamentSimpleCollection
{
    public required List<UserMedicamentSimpleModel> Medicaments { get; set; }
}