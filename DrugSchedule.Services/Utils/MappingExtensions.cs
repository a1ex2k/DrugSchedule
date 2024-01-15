using DrugSchedule.BusinessLogic.Models;

namespace DrugSchedule.BusinessLogic.Utils;

public static class MappingExtensions
{
    public static FileInfoModel ToFileInfoModel(this FileInfo fileInfo)
    {
        return new FileInfoModel
        {
            Guid = fileInfo.Guid,
            NameWithExtension = string.Concat(fileInfo.OriginalName, fileInfo.FileExtension),
            MediaType = fileInfo.MediaType,
            Size = fileInfo.Size,
            CreationTime = fileInfo.CreatedAt
        };
    }
}