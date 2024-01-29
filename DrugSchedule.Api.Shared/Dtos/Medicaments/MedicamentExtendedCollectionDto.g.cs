using System.Collections.Generic;
using DrugSchedule.BusinessLogic.Models;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class MedicamentExtendedCollectionDto
    {
        public List<MedicamentExtendedModel> Medicaments { get; set; }
    }
}