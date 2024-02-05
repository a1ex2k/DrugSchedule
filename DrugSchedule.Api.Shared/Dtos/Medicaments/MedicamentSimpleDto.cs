namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class MedicamentSimpleDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required string ReleaseForm { get; set; }
        public string? ManufacturerName { get; set; }
    }
}