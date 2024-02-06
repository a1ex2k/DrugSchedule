using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos
{
    public class MedicamentExtendedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Composition { get; set; }
        public string? Description { get; set; }
        public MedicamentReleaseFormDto ReleaseForm { get; set; }
        public ManufacturerDto? Manufacturer { get; set; }
        public List<DownloadableFileDto> Images { get; set; }
    }
}