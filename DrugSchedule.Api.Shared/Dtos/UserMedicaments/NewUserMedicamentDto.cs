namespace DrugSchedule.Api.Shared.Dtos
{
    public class NewUserMedicamentDto
    {
        public int? BasicMedicamentId { get; set; }
        public required string Name { get; set; }
        public required string ReleaseForm { get; set; }
        public string? Description { get; set; }
        public string? Composition { get; set; }
        public string? ManufacturerName { get; set; }
    }
}