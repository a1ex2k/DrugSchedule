using System;

namespace DrugScheduleFill;

public class MedicamentFile
{
    public long Id { get; set; }

    public int MedicamentId { get; set; }

    public required Guid FileGuid { get; set; }
}