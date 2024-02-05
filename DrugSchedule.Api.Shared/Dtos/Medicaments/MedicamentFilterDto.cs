using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class MedicamentFilterDto
    {
        public List<int>? IdFilter { get; set; }
        public StringFilterDto? NameFilter { get; set; }
        public ManufacturerFilterDto? ManufacturerFilter { get; set; }
        public MedicamentReleaseFormFilterDto? MedicamentReleaseFormFilter { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}