using System;

namespace DrugSchedule.StorageContract.Data;

public class TakingScheduleSimple
{
    public long Id { get; set; }

    public UserContactSimple? ContactOwner { get; set; }

    public string? MedicamentName { get; set; }

    public string? MedicamentReleaseFormName { get; set; }

    public FileInfo? MedicamentImage { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }
}