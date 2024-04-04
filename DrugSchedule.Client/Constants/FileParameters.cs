namespace DrugSchedule.Client.Constants;

public static class FileParameters
{
    public const int MaxAvatarFileSize = 5 * 1024 * 1024;
    public static readonly string[] AvatarMimeTypes = ["image/jpeg", "image/png"];
    public const int MaxUserMedicamentFileSize = 5 * 1024 * 1024;
    public static readonly string[] UserMedicamentFileMimeTypes = ["image/jpeg", "image/png", "image/gif"];
    public const int MaxConfirmationFileSize = 10 * 1024 * 1024;
    public static readonly string[] UserConfirmationFileMimeTypes = ["image/jpeg", "image/png", "image/gif"];
}