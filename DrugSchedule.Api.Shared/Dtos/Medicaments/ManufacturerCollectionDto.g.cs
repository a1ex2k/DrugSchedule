using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class ManufacturerCollectionDto
    {
        public List<ManufacturerDto> Manufacturers { get; set; }
    }
}