namespace DrugSchedule.SqlServer.Data.Entities;

public class ScheduleShare
{
    public required long MedicamentTakingScheduleId { get; set; }

    public MedicamentTakingSchedule? MedicamentTakingSchedule { get; set; }

    public required long ShareWithProfileId { get; set; }

    public UserProfile? ShareWithProfile { get; set; }
}