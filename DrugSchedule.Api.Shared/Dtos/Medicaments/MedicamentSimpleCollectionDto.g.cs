using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class MedicamentSimpleCollectionDto
    {
        public List<MedicamentSimpleDto> Medicaments { get; set; }
    }
}