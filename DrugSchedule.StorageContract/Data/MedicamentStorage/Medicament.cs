using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class Medicament
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public int? PackQuantity { get; set; }

    public string? Dosage { get; set; }

    public MedicamentReleaseForm? ReleaseForm { get; set; }

    public Manufacturer? Manufacturer { get; set; }

    public List<FileInfo>? Images { get; set; }
}