using System;

namespace DrugSchedule.Api.Shared.Dtos;

public class ConfirmationImageRemoveDto
{
    public long ConfirmationId { get; set; }

    public long RepeatId { get; set; }

    public Guid FileGuid { get; set; }
}