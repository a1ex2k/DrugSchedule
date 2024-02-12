using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class UserMedicamentExtendedModel
{
    public long Id { get; set; }

    public MedicamentExtendedModel? BasicMedicament { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string? Composition { get; set; }

    public string ReleaseForm { get; set; } = default!;

    public string? ManufacturerName { get; set; }

    public required FileCollection Images { get; set; } 
}