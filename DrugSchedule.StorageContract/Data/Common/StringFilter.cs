namespace DrugSchedule.StorageContract.Data;

public class StringFilter
{
    public required string SubString { get; set; }

    public required StringSearch StringSearchType { get; set; }
}