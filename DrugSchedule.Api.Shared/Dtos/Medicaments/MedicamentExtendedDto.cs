using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos
{
    public class MedicamentExtendedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Composition { get; set; }
        public string? Description { get; set; }
        public MedicamentReleaseFormDto ReleaseForm { get; set; } = default!;
        public ManufacturerDto? Manufacturer { get; set; }
        public FileCollectionDto FileCollection { get; set; } = default!;
    }
}