namespace DrugSchedule.Api.Shared.Dtos
{
    public class UserMedicamentExtendedDto
    {
        public long Id { get; set; }
        public MedicamentExtendedDto? BasicMedicament { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Composition { get; set; }
        public required string ReleaseForm { get; set; }
        public string? ManufacturerName { get; set; }
        public required FileCollectionDto Images { get; set; }
    }
}