using System.Collections.Generic;
using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class ManufacturerFilterDto
    {
        public List<int>? IdFilter { get; set; }
        public StringFilterDto? NameFilter { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}