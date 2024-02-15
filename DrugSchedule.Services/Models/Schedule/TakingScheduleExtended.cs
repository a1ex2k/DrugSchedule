using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class TakingScheduleExtended
{
    public long Id { get; set; }

    public UserContactSimple? ContactOwnerProfile { get; set; }

    public MedicamentSimple? GlobalMedicament { get; set; }

    public UserMedicamentSimple? UserMedicament { get; set; }

    public string? Information { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }

    public List<ScheduleRepeatPlain> ScheduleRepeats { get; set; } = new();

    public List<ScheduleShareExtended> ScheduleShares { get; set; } = new();
}