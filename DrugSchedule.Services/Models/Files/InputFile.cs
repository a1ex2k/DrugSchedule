namespace DrugSchedule.BusinessLogic.Models;

public class InputFile 
{
    public required string NameWithExtension { get; set; }

    public required string MediaType { get; set; }

    public required Stream Stream { get; set; }
}