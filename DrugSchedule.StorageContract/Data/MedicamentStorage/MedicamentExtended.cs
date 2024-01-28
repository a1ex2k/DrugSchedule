using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class MedicamentExtended
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Composition { get; set; }

    public string? Description { get; set; }

    public required MedicamentReleaseForm ReleaseForm { get; set; }

    public required Manufacturer? Manufacturer { get; set; }

    public required List<FileInfo>? Images { get; set; }
}