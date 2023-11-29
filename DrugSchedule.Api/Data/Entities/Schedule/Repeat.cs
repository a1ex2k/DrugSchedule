﻿namespace DrugSchedule.Api.Data;

public class Repeat
{
    public long Id { get; set; }

    public required int UserProfileId { get; set; }

    public UserProfile UserProfile { get; set; }
    
    public required DateOnly BeginDate { get; set; }

    public required TimeOnly Time { get; set; }

    public required TimeOfDay TimeOfDay { get; set; }

    public required RepeatDayOfWeek RepeatDayOfWeek { get; set; }

    public required DateOnly EndDate { get; set; }
    
    public required int TakingScheduleId { get; set; }

    public MedicamentTakingSchedule MedicamentTakingSchedule { get; set; }
}