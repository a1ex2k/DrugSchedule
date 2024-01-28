namespace DrugSchedule.Api.FileAccessProvider;

public class FileAccessParams
{
    public Guid FileGuid { get; set; }
    public string AccessKey { get; set; }
    public long ExpiryTime { get; set; }
    public string Signature { get; set; }
}