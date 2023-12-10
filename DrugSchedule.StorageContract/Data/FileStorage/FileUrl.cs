using System;


namespace DrugSchedule.StorageContract.Data;

public class FileUrl
{
    public required Guid FileGuid { get; set; }

    public required Uri SignedUrl { get; set; }
}