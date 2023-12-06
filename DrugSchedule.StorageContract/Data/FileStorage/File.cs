using System.IO;

namespace DrugSchedule.StorageContract.Data.FileStorage;

public class File
{
    public required FileInfo FileInfo { get; set; }

    public required Stream Stream { get; set; }
}