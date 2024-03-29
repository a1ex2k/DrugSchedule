﻿namespace DrugSchedule.Services.Models;

public class AwaitableFileParams
{
    public required long MaxSize { get; set; }

    public required string[] FileExtensions { get; set; }

    public required bool TryCreateThumbnail { get; set; }
}
