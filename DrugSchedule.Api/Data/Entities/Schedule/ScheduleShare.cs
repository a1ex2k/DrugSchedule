namespace DrugSchedule.Api.Data;

public class ScheduleShare
{
    public required long MedicamentTakingScheduleId { get; set; }

    public MedicamentTakingSchedule MedicamentTakingSchedule { get; set; }

    public required int ShareWithProfileId { get; set; }

    public UserProfile ShareWithProfile { get; set; }
}