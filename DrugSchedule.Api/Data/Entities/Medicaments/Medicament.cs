namespace DrugSchedule.Api.Data;

public class Medicament
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required int PackQuantity { get; set; }

    public required string Dosage { get; set; }

    public required int ReleaseFormId { get; set; }

    public MedicamentReleaseForm ReleaseForm { get; set; }

    public required int ManufacturerId { get; set; }

    public Manufacturer Manufacturer { get; set; }
}