using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Storage;

public class FileStorageOptions
{
    public const string Title = "FileStorageOptions";

    [Required]
    public string DirectoryPath { get; set; } = String.Empty;
}