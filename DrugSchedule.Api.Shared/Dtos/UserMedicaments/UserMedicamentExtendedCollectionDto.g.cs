using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class UserMedicamentExtendedCollectionDto
    {
        public List<UserMedicamentExtendedDto> Medicaments { get; set; }
    }
}