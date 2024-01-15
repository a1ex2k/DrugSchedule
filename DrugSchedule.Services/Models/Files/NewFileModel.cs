namespace DrugSchedule.BusinessLogic.Models;

public class NewFileModel {
    public required string NameWithExtension { get; set; }

    public required string MediaType { get; set; }

    public required Stream Stream { get; set; }
}