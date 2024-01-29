using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class ReleaseFormCollectionDto
    {
        public List<MedicamentReleaseFormDto> ReleaseForms { get; set; }
    }
}