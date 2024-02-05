using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class MedicamentExtendedCollectionDto
    {
        public List<MedicamentExtendedDto> Medicaments { get; set; }
    }
}