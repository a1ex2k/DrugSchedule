namespace DrugSchedule.Client.Utils;

public class EnumElement<T> where T : Enum
{
    public string Name { get; init; }
    public T Value { get; init; }

    public EnumElement(string name, T value)
    {
        Name = name;
        Value = value;
    }
}