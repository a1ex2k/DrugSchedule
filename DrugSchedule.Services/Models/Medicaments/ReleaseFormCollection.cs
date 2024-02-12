using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class ReleaseFormCollection
{
    public required List<MedicamentReleaseForm> ReleaseForms { get; set; }
}