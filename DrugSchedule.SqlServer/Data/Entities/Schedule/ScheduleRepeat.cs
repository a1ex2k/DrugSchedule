namespace DrugSchedule.SqlServer.Data.Entities;

public class ScheduleRepeat
{ 
    public long Id { get; set; }

    public required long MedicamentTakingScheduleId { get; set; }
    
    public MedicamentTakingSchedule? MedicamentTakingSchedule { get; set; }

    public required long RepeatId { get; set; }

    public Repeat? Repeat { get; set; }

    public required long TakingRuleId { get; set; }
    
    public TakingRule? TakingRule { get; set; }
}