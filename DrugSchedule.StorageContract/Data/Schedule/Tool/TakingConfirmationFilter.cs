using System;

namespace DrugSchedule.StorageContract.Data;

public class TakingConfirmationFilter : FilterBase
{
    public required long RepeatId { get; set; }

    public required DateOnly LastDate { get; set; }

    public DateOnly? FirstDate { get; set; }
}