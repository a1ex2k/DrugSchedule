﻿using System;
using System.Collections.Generic;

namespace DrugSchedule.StorageContract.Data;

public class MedicamentSimple
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string ReleaseForm { get; set; }

    public string? ManufacturerName { get; set; }

    public required FileInfo? MainImage { get; set; }
}