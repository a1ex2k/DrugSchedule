namespace DrugSchedule.Storage.Data.Entities;

public class MedicamentTakingSchedule
{
    public long Id { get; set; }

    public required long UserProfileId { get; set; }

    public UserProfile? UserProfile { get; set; }

    public long? UserMedicamentId { get; set; }

    public UserMedicament? UserMedicament { get; set; }

    public string? Information { get; set; }

    public required DateTime CreationTime { get; set; }

    public List<ScheduleRepeat> RepeatSchedules { get; set; } = new();

    public List<UserProfileContact> SharedWith { get; set; } = new();
}