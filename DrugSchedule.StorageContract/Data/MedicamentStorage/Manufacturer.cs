namespace DrugSchedule.StorageContract.Data.MedicamentStorage;

public class Manufacturer
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? AdditionalInfo { get; set; }
}