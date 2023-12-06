namespace DrugSchedule.SqlServer.Data.Entities;

public class UserMedicament
{
    public long Id { get; set; }

    public int? BasedOnMedicamentId { get; set; }

    public Medicament? BasedOnMedicament { get; set; }

    public string? Name { get; set; }

    public int? PackQuantity { get; set; }

    public string? Dosage { get; set; }

    public string? ReleaseForm { get; set; }

    public string? ManufacturerName { get; set; }

    public required int UserProfileId { get; set; }

    public Guid? ImageGuid { get; set; }
}