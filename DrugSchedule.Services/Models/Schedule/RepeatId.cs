namespace DrugSchedule.Services.Models;

public record struct RepeatId
{
    public long Value { get; init; }

    public static implicit operator RepeatId(long value)
    {
        return new RepeatId { Value = value };
    }

    public static implicit operator long(RepeatId wrapper)
    {
        return wrapper.Value;
    }
}