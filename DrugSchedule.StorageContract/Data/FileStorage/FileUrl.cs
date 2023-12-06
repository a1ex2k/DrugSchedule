using System;


namespace DrugSchedule.StorageContract.Data.FileStorage;

public class FileUrl
{
    public required Guid FileGuid { get; set; }

    public required Uri SignedUrl { get; set; }
}