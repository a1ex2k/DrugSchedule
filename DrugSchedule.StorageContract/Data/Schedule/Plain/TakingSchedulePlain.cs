using System;

namespace DrugSchedule.StorageContract.Data;

public class TakingSchedulePlain
{
    public long Id { get; set; }

    public required long UserProfileId { get; set; }

    public int? GlobalMedicamentId { get; set; }

    public long? UserMedicamentId { get; set; }

    public string? Information { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }
}