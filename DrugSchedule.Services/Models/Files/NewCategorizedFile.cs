using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class NewCategorizedFile
{
    public required string NameWithExtension { get; set; }

    public required FileCategory Category { get; set; }

    public required string MediaType { get; set; }

    public required Stream Stream { get; set; }
}