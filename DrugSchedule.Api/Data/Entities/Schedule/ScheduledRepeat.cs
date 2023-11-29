namespace DrugSchedule.Api.Data;

public class ScheduledRepeat
{
    public long Id { get; set; }

    public required int MedicamentTakingScheduleId { get; set; }
    
    public MedicamentTakingSchedule MedicamentTakingSchedule { get; set; }

    public required int RepeatId { get; set; }

    public Repeat Repeat { get; set; }

    public List<ComputedSchedule> ComputedSchedules { get; set; }
}