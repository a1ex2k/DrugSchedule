using System;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Api.Shared.Dtos;

public class NewTakingСonfirmationDto
{
    public long RepeatId { get; set; }

    public DateOnly ForDate { get; set; }

    public TimeOnly? ForTime { get; set; }

    public TimeOfDayDto ForTimeOfDay { get; set; }

    public string? Text { get; set; }
}