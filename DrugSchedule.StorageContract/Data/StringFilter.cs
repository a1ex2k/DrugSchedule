using System;

namespace DrugSchedule.StorageContract.Data;

public class StringFilter
{
    public required string SubString { get; set; }

    public required StringComparison StringComparisonType { get; set; }
}