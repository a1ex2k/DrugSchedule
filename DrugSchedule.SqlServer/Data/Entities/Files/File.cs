using Microsoft.EntityFrameworkCore;
using System;

namespace DrugSchedule.SqlServer.Data.Entities;

[Index(nameof(Guid), Name = "FileGuid")]
public class FileInfo
{
    public int Id { get; set; }

    public required Guid Guid { get; set; }

    public required string FileName { get; set; }

    public required string ContentType { get; set; }

    public required long Size { get; set; }

    public required DateTime CreatedAt { get; set; }
}