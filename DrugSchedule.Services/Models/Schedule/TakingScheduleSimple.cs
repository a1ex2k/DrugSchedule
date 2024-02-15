using DrugSchedule.Services.Models;

namespace DrugSchedule.StorageContract.Data;

public class TakingScheduleSimple
{
    public long Id { get; set; }

    public string? MedicamentName { get; set; }

    public string? MedicamentReleaseFormName { get; set; }

    public string? ThumbnailUrl { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }
}