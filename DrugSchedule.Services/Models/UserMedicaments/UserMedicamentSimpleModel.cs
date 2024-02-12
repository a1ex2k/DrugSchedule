using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class UserMedicamentSimpleModel
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public string ReleaseForm { get; set; } = default!;

    public string? ManufacturerName { get; set; }

    public string? ThumbnailUrl { get; set; }
}