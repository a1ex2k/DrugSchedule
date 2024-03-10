namespace DrugSchedule.Client.Constants;

public static class Credentials
{
    public const string UsernamePattern = @"^(?=.*[A-Za-z])[A-Za-z\d_]{4,32}$";
    public const string UsernameTaken = "Username already taken";
    public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&+~|{}:;<>/])[A-Za-z\d$@$!%*?&+~|{}:;<>/]{8,32}";
    public const string UsernameRequirements = "At least one letter, digits, underscores. 4 to 32 characters";
    public const string PasswordRequirements = "At least one uppercase letter, one lowercase letter, one special character, one digit. 8 to 32 characters";
    public const string FormInvalid = "Values not valid";
    public const string EmailInvalid = "Email not valid";
    public const int MaxLength = 32;
}