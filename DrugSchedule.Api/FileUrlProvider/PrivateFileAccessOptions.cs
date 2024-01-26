namespace DrugSchedule.Api.FileUrlProvider;

public class PrivateFileAccessOptions
{
    public const string Title = "PrivateFileAccess";

    public int LinkExpirationInSeconds { get; set; }

    public required string SecretKey { get; set; }
}

