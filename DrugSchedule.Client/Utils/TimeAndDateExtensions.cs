using System.Globalization;
using DrugSchedule.Client.Constants;

namespace DrugSchedule.Client.Utils;

public static class TimeAndDateExtensions
{
    private static readonly IFormatProvider FormatProvider = new CultureInfo("en-US");

    public static string ToLongString(this DateOnly date)
        => date.ToString(date.Year == DateTime.Now.Year ? DateAndTime.DateLongFormatCurrentYear : DateAndTime.DateLongFormat, FormatProvider);   
    
    public static string ToShortString(this DateOnly date)
        => date.ToString(date.Year == DateTime.Now.Year ? DateAndTime.DateShortFormatCurrentYear : DateAndTime.DateShortFormat, FormatProvider);

    public static string ToLongString(this TimeOnly time)
        => time.ToString(DateAndTime.TimeLongFormat);

    public static string ToShortString(this TimeOnly time)
        => time.ToString(DateAndTime.TimeShortFormat);
    public static string ToLongString(this DateTime dateTime)
        => dateTime.ToLocalTime().ToString(dateTime.Year == DateTime.Now.Year ? DateAndTime.DateTimeLongFormatCurrentYear : DateAndTime.DateTimeLongFormat, FormatProvider);

    public static string ToShortString(this DateTime dateTime)
        => dateTime.ToLocalTime().ToString(dateTime.Year == DateTime.Now.Year ? DateAndTime.DateTimeShortFormatCurrentYear : DateAndTime.DateTimeShortFormat, FormatProvider);
}