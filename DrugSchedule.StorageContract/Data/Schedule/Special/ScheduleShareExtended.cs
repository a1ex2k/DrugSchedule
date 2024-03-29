﻿namespace DrugSchedule.StorageContract.Data;

public class ScheduleShareExtended
{
    public long Id { get; set; }

    public UserContactSimple UserContact { get; set; } = default!;

    public string? Comment { get; set; }
}