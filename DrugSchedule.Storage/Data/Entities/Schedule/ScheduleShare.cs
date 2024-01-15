namespace DrugSchedule.Storage.Data.Entities;

public class ScheduleShare
{
    public required long MedicamentTakingScheduleId { get; set; }

    public MedicamentTakingSchedule? MedicamentTakingSchedule { get; set; }

    public required long ShareWithContactId { get; set; }

    public UserProfileContact? ShareWithContact { get; set; }
}