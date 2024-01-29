using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Api.Shared.Dtos
{
    public partial class StringFilterDto
    {
        public string SubString { get; set; }
        public StringSearch StringSearchType { get; set; }
    }
}