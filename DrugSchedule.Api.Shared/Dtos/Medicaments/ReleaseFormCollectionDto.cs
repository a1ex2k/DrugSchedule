using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public class ReleaseFormCollectionDto
    {
        public List<MedicamentReleaseFormDto> ReleaseForms { get; set; }
    }
}