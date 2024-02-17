namespace DrugSchedule.Services.Models;

public class UserMedicamentUpdate
{
    public long Id { get; set; }

    public int? BasicMedicamentId { get; set; }

    public string Name { get; set; } = default!;

    public string ReleaseForm { get; set; } = default!;

    public string? Description { get; set; }

    public string? Composition { get; set; }

    public string? ManufacturerName { get; set; }
}