using System;

namespace DrugScheduleFill.Models;

public class MedicamentFile
{
    public long Id { get; set; }

    public int MedicamentId { get; set; }

    public required Guid FileGuid { get; set; }
}