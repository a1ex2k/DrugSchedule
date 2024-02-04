namespace DrugSchedule.StorageContract.Data;

public class UserMedicamentUpdateFlags
{
    public bool BasedOnMedicament { get; set; }

    public bool Name { get; set; } = default!;

    public bool Description { get; set; }

    public bool Composition { get; set; }

    public bool ReleaseForm { get; set; }

    public bool ManufacturerName { get; set; }

    public bool Images { get; set; }
}