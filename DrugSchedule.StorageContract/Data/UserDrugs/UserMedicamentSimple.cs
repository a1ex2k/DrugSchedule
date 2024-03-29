﻿namespace DrugSchedule.StorageContract.Data;

public class UserMedicamentSimple
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public string ReleaseForm { get; set; } = default!;

    public string? ManufacturerName { get; set; }

    public FileInfo? MainImage { get; set; }
}