using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class TakingСonfirmationDto
{
    public required long Id { get; set; }

    public required long RepeatId { get; set; }

    public DateOnly ForDate { get; set; }

    public TimeOnly? ForTime { get; set; }

    public TimeOfDayDto ForTimeOfDay { get; set; }

    public required DateTime CreatedAt { get; set; }

    public FileCollectionDto Images { get; set; } = default!;

    public string? Text { get; set; }
}