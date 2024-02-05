using System.Collections.Generic;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class UserMedicamentFilterDto
    {
        public List<long>? IdFilter { get; set; }
        public StringFilterDto? NameFilter { get; set; }
        public StringFilterDto? ManufacturerNameFilter { get; set; }
        public StringFilterDto? ReleaseFormNameFilter { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}