using System.IO;

namespace DrugSchedule.StorageContract.Data;

public class File
{
    public required FileInfo FileInfo { get; set; }

    public required Stream Stream { get; set; }
}