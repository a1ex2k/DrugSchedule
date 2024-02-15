﻿using System;
using System.Collections.Generic;
using DrugSchedule.Services.Models;

namespace DrugSchedule.StorageContract.Data;

public class TakingСonfirmation
{
    public required long Id { get; set; }

    public required long RepeatId { get; set; }

    public required DateTime CreatedAt { get; set; }

    public FileCollection Images { get; set; } = default!;

    public string? Text { get; set; }
}