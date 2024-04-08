using System.Globalization;
using DrugSchedule.Client.Constants;

namespace DrugSchedule.Client.Utils;

public static class TimeAndDateExtensions
{
    private static readonly IFormatProvider FormatProvider = new CultureInfo("en-US");

    public static string ToLongString(this DateOnly date)
        => date.ToString(Schedules.DateLongFormat, FormatProvider);   
    
    public static string ToShortString(this DateOnly date)
        => date.ToString(Schedules.DateShortFormat, FormatProvider);

    public static string ToLongString(this TimeOnly time)
        => time.ToString(Schedules.TimeLongFormat);

    public static string ToShortString(this TimeOnly time)
        => time.ToString(Schedules.TimeShortFormat);

    public static string ToLongString(this DateTime dateTime)
        => dateTime.ToString(Schedules.DateTimeLongFormat, FormatProvider);

    public static string ToShortString(this DateTime dateTime)
        => dateTime.ToString(Schedules.DateTimeShortFormat, FormatProvider);
}