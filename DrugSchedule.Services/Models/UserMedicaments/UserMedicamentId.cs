namespace DrugSchedule.Services.Models;

public record struct UserMedicamentId
{
    public long Value { get; init; }

    public static implicit operator UserMedicamentId(long value)
    {
        return new UserMedicamentId { Value = value };
    }

    public static implicit operator long(UserMedicamentId wrapper)
    {
        return wrapper.Value;
    }
}