namespace DrugScheduleFill.Models;

public class Medicament
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Composition { get; set; }

    public required int ReleaseFormId { get; set; }

    public required int? ManufacturerId { get; set; }

}