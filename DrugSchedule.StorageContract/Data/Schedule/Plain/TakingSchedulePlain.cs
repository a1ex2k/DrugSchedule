using System;

namespace DrugSchedule.StorageContract.Data;

public class TakingSchedulePlain
{
    public long Id { get; set; }

    public long UserProfileId { get; set; }

    public int? GlobalMedicamentId { get; set; }

    public long? UserMedicamentId { get; set; }

    public string? Information { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Enabled { get; set; }
}