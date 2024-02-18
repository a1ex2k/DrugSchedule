namespace DrugSchedule.Services.Models;

public record struct ScheduleId
{
    public long Value { get; init; }

    public static implicit operator ScheduleId(long value)
    {
        return new ScheduleId { Value = value };
    }

    public static implicit operator long(ScheduleId wrapper)
    {
        return wrapper.Value;
    }
}