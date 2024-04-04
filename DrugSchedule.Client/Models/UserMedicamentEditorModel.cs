using Blazorise;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Models;

public class UserMedicamentEditorModel
{
    public MedicamentSimpleDto? BasicMedicament { get; set; }
    public string Name { get; set; } = default!;
    public string ReleaseForm { get; set; } = default!;
    public string? Description { get; set; }
    public string? Composition { get; set; }
    public string? ManufacturerName { get; set; }
    public List<DownloadableFileDto> ExistingImages { get; set; } = new();
    public List<IFileEntry> NewImages { get; set; } = new();
    public List<DownloadableFileDto> DeleteImages { get; set; } = new();
}