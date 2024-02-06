namespace DrugSchedule.Api.Shared.Dtos
{
    public class StringFilterDto
    {
        public required string SubString { get; set; }
        public StringSearchDto StringSearchType { get; set; }
    }
}