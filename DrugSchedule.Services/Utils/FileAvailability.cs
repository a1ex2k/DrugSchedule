using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Utils;

public static class FileAvailability
{
    private static AwaitableFileParams UserAvatarFileParams => new AwaitableFileParams
    {
        MaxSize = 2 * 1024 * 1024,
        FileExtensions = new[] { ".jpeg", ".jpg", ".png" }
    };

    private static AwaitableFileParams UserMedicamentFileParams => new AwaitableFileParams
    {
        MaxSize = 2 * 1024 * 1024,
        FileExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" }
    };

    private static AwaitableFileParams TakingConfirmationFileParams => new AwaitableFileParams
    {
        MaxSize = 7 * 1024 * 1024,
        FileExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" }
    };

    private static AwaitableFileParams MedicamentImageFileParams => new AwaitableFileParams
    {
        MaxSize = 800 * 1024,
        FileExtensions = new[] { ".jpeg", ".jpg", ".png", ".gif" }
    };


    public static bool IsPublic(this FileCategory category)
    {
        return category switch
        {
            FileCategory.None => false,
            FileCategory.MedicamentImage => true,
            FileCategory.UserAvatar => false,
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
            FileCategory.MedicamentImage => MedicamentImageFileParams,
            FileCategory.UserAvatar => UserAvatarFileParams,
            FileCategory.DrugConfirmation => TakingConfirmationFileParams,
            FileCategory.UserMedicamentImage => UserMedicamentFileParams,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}