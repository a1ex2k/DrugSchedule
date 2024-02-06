using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos
{
    public class MedicamentFilterDto
    {
        public List<int>? IdFilter { get; set; }
        public StringFilterDto? NameFilter { get; set; }
        public ManufacturerFilterDto? ManufacturerFilter { get; set; }
        public MedicamentReleaseFormFilterDto? MedicamentReleaseFormFilter { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}