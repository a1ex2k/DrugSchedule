namespace DrugSchedule.Services.Utils;

internal static class ErrorMessages
{
    public const string BasicMedicamentNotFound = "Basic medicament not found";
    public const string CannotRemoveFile = "Cannot remove file";
    public const string ConfirmationNotMeetTimetable = "Confirmation parameters don't meet repeat's timetable";
    public const string EmailOrUsernameIsUsed = "Email or username is already used";
    public const string EmptyGuid = "Empty guid";
    public const string FileInfoNotFound = "File info not found";
    public const string FilenameNotSet = "Original filename not set";
    public const string FileNotFound = "File not found";
    public const string FileThumbnailNotFound = "File thumbnail not found";
    public const string IncorrectOldPassword = "Old password doesn't match or same to new one";
    public const string IncorrectUsernamePassword = "Either username or password is incorrect";
    public const string InvalidDateOfBirth = "Invalid date of birth. Current age must be greater than 5 and less than 120";
    public const string InvalidEmail = "Email is invalid";
    public const string InvalidRealName = "Invalid real name. Max 30 characters";
    public const string InvalidRequestScheduleDates = "Invalid dates. Minimum and maximum dates difference must be 32 days or less";
    public const string ManufacturerNotFound = "Manufacturer not found";
    public const string MedicamentNotFound = "Medicament not found";
    public const string MimeTypeNotSet = "MIME type not set";
    public const string NameMustBeNonWhiteSpace = "Name must be non white space";
    public const string NoCommonContact = "User doesn't have a common contact with specified one";
    public const string NoContact = "No contact with specified user";
    public const string PasswordUsernameMustDiffer = "Password and username must differ";
    public const string ReleaseFormMustBeNonWhitespace = "Release form name must be non white space";
    public const string ReleaseFormNotFound = "Release form not found";
    public const string RepeatCannotBeUpdated = "Repeat cannot be updated because it already has confirmations. Create another one";
    public const string ScheduleDatesInvalid = "End date must be undefined or greater than begin date";
    public const string ScheduleMedicamentsInvalid = "Either global medicament or user's medicament must be defined";
    public const string ScheduleNotFound = "Schedule not found";
    public const string ScheduleNotFoundOrNoPermissionsToAccess = "Schedule was not found or user doesn't have permissions to access";
    public const string SearchValueMustBeThreeChars = "Search value must be at least 3 not whitespace characters long";
    public const string SharedUserMedicamentNotFoundOrNoPermissions = "Shared user medicament not found or current user doesn't have permissions to access";
    public const string StreamCannotBeRead = "Input data stream cannot be read";
    public const string RepeatDaysInvalid = "At least single day of week must be defined";
    public const string UserDoesntHaveConfirmation = "User doesn't have specified confirmation";
    public const string UserDoesntHaveMedicament = "User doesn't have specified medicament";
    public const string UserDoesntHaveRepeat = "User doesn't have specified repeat";
    public const string UserDoesntHaveSchedule = "User doesn't have specified schedule";
    public const string UserMedicamentNotFound = "User medicament not found";
    public const string UserMedicamentReferenced = "User medicament cannot be removed because it is referenced by another object";
    public const string UserNotFound = "User not found";
}