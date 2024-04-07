namespace DrugSchedule.Client.Utils;

public class FlagEnumElement<T> where T : Enum
{
    public string Name { get; init; }
    public T Value { get; init; }
    public bool Checked { get; set; }

    public FlagEnumElement(string name, T value, bool isChecked)
    {
        Name = name;
        Value = value;
        Checked = isChecked;
    }
}