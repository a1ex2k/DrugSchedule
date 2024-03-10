using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public class UserMedicamentSimpleCollectionDto
    {
        public List<UserMedicamentSimpleDto> Medicaments { get; set; }
    }
}