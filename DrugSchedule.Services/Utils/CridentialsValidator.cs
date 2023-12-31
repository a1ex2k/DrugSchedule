﻿using System.Text.RegularExpressions;

namespace DrugSchedule.BusinessLogic.Utils;

public static class CridentialsValidator
{
    public const int MaxPasswordLength = 32;
    public const int MaxUsernameLength = 20;

    public const string PasswordRegexString = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{5,}$";
    public const string UsernameRegexString = @"^[A-Za-z\d_][A-Za-z\d]{3,}$";

    private static readonly Regex PasswordRegex =
        new Regex(PasswordRegexString, RegexOptions.Compiled);
    private static readonly Regex UsernameRegex =
        new Regex(UsernameRegexString, RegexOptions.Compiled);

    public static bool ValidatePassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length > MaxPasswordLength)
        {
            return false;
        }

        return PasswordRegex.IsMatch(password);
    }

    public static bool ValidateUsername(string? username)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length > MaxUsernameLength)
        {
            return false;
        }

        return UsernameRegex.IsMatch(username);
    }
}