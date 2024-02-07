namespace DrugSchedule.Storage.Data.Entities;

public class MedicamentTakingSchedule
{
    public long Id { get; set; }

    public required long UserProfileId { get; set; }

    public UserProfile? UserProfile { get; set; }

    public int? GlobalMedicamentId { get; set; }

    public Medicament? GlobalMedicament { get; set; }

    public long? UserMedicamentId { get; set; }

    public UserMedicament? UserMedicament { get; set; }

    public string? Information { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool Enabled { get; set; }

    public List<ScheduleRepeat> RepeatSchedules { get; set; } = new();

    public List<ScheduleShare> ScheduleShares { get; set; } = new();
}