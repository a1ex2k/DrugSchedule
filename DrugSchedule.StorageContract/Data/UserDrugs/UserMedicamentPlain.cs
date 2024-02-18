using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class UserMedicamentPlain
{
    public long Id { get; set; }

    public int? BasicMedicamentId { get; set; }

    public string Name { get; set; } = default!;

    public string ReleaseForm { get; set; } = default!;

    public string? Description { get; set; }

    public string? Composition { get; set; }

    public string? ManufacturerName { get; set; }

    public List<Guid>? ImageGuids { get; set; }

    public long UserId { get; set; }
}