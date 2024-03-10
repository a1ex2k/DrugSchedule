using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public class UserMedicamentExtendedCollectionDto
    {
        public List<UserMedicamentExtendedDto> Medicaments { get; set; }
    }
}