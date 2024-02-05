namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class UserMedicamentSimpleDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ReleaseForm { get; set; }
        public string? ManufacturerName { get; set; }
        public DownloadableFileDto? MainImage { get; set; }
    }
}