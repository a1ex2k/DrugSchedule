using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class ReleaseFormCollection
{
    public required List<MedicamentReleaseForm> ReleaseForms { get; set; }
}