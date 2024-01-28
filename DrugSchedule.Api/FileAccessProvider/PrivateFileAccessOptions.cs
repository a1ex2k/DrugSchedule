using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.FileAccessProvider;

public class PrivateFileAccessOptions
{
    public const string Title = "PrivateFileAccess";

    [Required, Range(5, 36000)]
    public int ExpirationInSeconds { get; init; }

    [Required, MinLength(32)]
    public required string SecretKey { get; set; }
}

