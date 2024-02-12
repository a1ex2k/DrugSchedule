using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class MedicamentExtendedModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Composition { get; set; }

    public string? Description { get; set; }

    public required MedicamentReleaseForm ReleaseForm { get; set; }

    public required Manufacturer? Manufacturer { get; set; }

    public required FileCollection FileCollection { get; set; }
}