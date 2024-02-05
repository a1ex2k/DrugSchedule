namespace DrugSchedule.BusinessLogic.Models;

public class MedicamentSimpleModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string ReleaseForm { get; set; }

    public string? ManufacturerName { get; set; }

    public required DownloadableFile? MainImage { get; set; }
}