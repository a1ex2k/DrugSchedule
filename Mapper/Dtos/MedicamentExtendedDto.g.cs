using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class MedicamentExtendedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Composition { get; set; }
        public string? Description { get; set; }
        public MedicamentReleaseFormDto ReleaseForm { get; set; }
        public ManufacturerDto? Manufacturer { get; set; }
        public List<FileInfo?>? Images { get; set; }
    }
}