using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Utils;

public static class FileAvailability
{
    private static AwaitableFileParams UserAvatarFileParams => new AwaitableFileParams
    {
        MaxSize = 5 * 1024 * 1024,
        FileExtensions = new[] { ".jpeg", ".jpg", ".png" },
        TryCreateThumbnail = true,
    };

    private static AwaitableFileParams UserMedicamentFileParams => new AwaitableFileParams
    {
        MaxSize = 5 * 1024 * 1024,
        FileExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" },
        TryCreateThumbnail = true,
    };

    private static AwaitableFileParams TakingConfirmationFileParams => new AwaitableFileParams
    {
        MaxSize = 10 * 1024 * 1024,
        FileExtensions = new[] { ".jpeg", ".jpg", ".png" },
        TryCreateThumbnail = true,
    };


    public static bool IsPublic(this FileCategory category)
    {
        return category switch
        {
            FileCategory.None => false,
            FileCategory.MedicamentImage => true,
            FileCategory.UserAvatar => true,
            FileCategory.DrugConfirmation => false,
            FileCategory.UserMedicamentImage => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static bool IsPublic(this FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo);
        return fileInfo.Category.IsPublic();
    }

    public static AwaitableFileParams GetAwaitableParams(this FileCategory category)
    {
        return category switch
        {
            FileCategory.UserAvatar => UserAvatarFileParams,
            FileCategory.DrugConfirmation => TakingConfirmationFileParams,
            FileCategory.UserMedicamentImage => UserMedicamentFileParams,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}