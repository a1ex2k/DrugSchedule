using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class UserMedicamentExtended
{
    public long Id { get; set; }

    public MedicamentExtended? BasicMedicament { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string? Composition { get; set; }

    public string? ReleaseForm { get; set; }

    public string? ManufacturerName { get; set; }

    public List<FileInfo>? Images { get; set; }
}