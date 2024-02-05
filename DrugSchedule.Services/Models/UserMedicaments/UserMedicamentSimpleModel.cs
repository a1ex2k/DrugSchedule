using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class UserMedicamentSimpleModel
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public string ReleaseForm { get; set; } = default!;

    public string? ManufacturerName { get; set; }

    public DownloadableFile? MainImage { get; set; }
}