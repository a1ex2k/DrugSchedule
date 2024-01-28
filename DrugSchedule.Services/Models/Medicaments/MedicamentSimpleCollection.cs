using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class MedicamentSimpleCollection
{
    public required List<MedicamentSimple> Medicaments { get; set; }
}