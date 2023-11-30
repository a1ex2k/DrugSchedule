﻿namespace DrugSchedule.Api.Data;

public class Manufacturer
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? AdditionalInfo { get; set; }
}