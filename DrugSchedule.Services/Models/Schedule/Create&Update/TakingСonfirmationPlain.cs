using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;


public class TakingСonfirmationPlain
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Guid> ImagesGuids { get; set; } = new();

    public string? Text { get; set; }

    public long ScheduleRepeatId { get; set; }

    public DateOnly ForDate { get; set; }

    public TimeOnly ForTime { get; set; }

    public TimeOfDay ForTimeOfDay { get; set; }
}