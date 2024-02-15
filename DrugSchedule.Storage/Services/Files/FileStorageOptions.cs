using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Storage.Services;

public class FileStorageOptions
{
    public const string SectionName = "FileStorageOptions";

    [Required]
    public string DirectoryPath { get; set; } = String.Empty;
}