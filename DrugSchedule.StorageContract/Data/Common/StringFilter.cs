using System;
using DrugSchedule.StorageContract.Data.Enums;

namespace DrugSchedule.StorageContract.Data.Common;

public class StringFilter
{
    public required string SubString { get; set; }

    public required StringSearch StringSearchType { get; set; }
}