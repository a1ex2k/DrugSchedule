namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class UserMedicamentUpdateDto
    {
        public long Id { get; set; }
        public int? BasicMedicamentId { get; set; }
        public string? Name { get; set; }
        public string? ReleaseForm { get; set; }
        public string? Description { get; set; }
        public string? Composition { get; set; }
        public string? ManufacturerName { get; set; }
    }
}