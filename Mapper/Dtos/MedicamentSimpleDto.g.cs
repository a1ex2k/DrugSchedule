namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class MedicamentSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReleaseForm { get; set; }
        public string? Manufacturer { get; set; }
    }
}