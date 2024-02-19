using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.Storage.Data.Entities;

public class FileInfo
{
    public required Guid Guid { get; set; }

    public required string OriginalName { get; set; }

    public required string Extension { get; set; }

    public required Contract.FileCategory FileCategory { get; set; }

    public required string MediaType { get; set; }

    public required long Size { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required bool HasThumbnail { get; set; }
}