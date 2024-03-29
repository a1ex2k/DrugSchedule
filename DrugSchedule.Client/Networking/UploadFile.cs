namespace DrugSchedule.Client.Networking;

public class UploadFile
{
    public required string Name { get; set; }

    public required Stream Stream { get; set; }

    public required string ContentType { get; set; }
}