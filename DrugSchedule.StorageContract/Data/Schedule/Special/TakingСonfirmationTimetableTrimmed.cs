using System;

namespace DrugSchedule.StorageContract.Data;

public class TakingСonfirmationTimetableTrimmed
{
    public long Id { get; set; }

    public DateOnly ForDate { get; set; }

    public TimeOnly? ForTime { get; set; }

    public TimeOfDay ForTimeOfDay { get; set; }
}