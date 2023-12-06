using System;

namespace DrugSchedule.StorageContract.Data.MedicamentStorage;

public class Medicament
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int? PackQuantity { get; set; }

    public string? Dosage { get; set; }

    public MedicamentReleaseForm? ReleaseForm { get; set; }

    public Manufacturer? Manufacturer { get; set; }

    public required Guid? ImageGuid { get; set; }
}