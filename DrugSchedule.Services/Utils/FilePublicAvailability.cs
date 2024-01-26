using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Utils;

public static class FilePublicAvailability
{
    public static bool IsPublic(this FileCategory category)
    {
        return category switch
        {
            FileCategory.None => false,
            FileCategory.MedicamentImage => true,
            FileCategory.MedicamentInstruction => true,
            FileCategory.UserAvatar => true,
            FileCategory.DrugConfirmation => false,
            FileCategory.UserMedicamentImage => false,
            _ => false
        };
    }

    public static bool IsPublic(this FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(fileInfo);
        return fileInfo.Category.IsPublic();
    }
}