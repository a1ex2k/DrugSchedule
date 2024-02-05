using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class NewUserMedicamentDto
    {
        public int? BasicMedicamentId { get; set; }
        public string Name { get; set; }
        public string ReleaseForm { get; set; }
        public string? Description { get; set; }
        public string? Composition { get; set; }
        public string? ManufacturerName { get; set; }
    }
}