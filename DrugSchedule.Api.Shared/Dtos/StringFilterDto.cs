namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class StringFilterDto
    {
        public string SubString { get; set; }
        public StringSearchDto StringSearchType { get; set; }
    }
}