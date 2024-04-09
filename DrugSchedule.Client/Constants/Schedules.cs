using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Utils;

namespace DrugSchedule.Client.Constants;

public static class Schedules
{
    public static readonly EnumElement<TimeOfDayDto>[] TimeOfDay = EnumHumanizeExtensions.TimeOfDayToArray();
    public const string DateLongFormat = "dddd, MMMM d, yyyy";
    public const string DateShortFormat = "ddd, MMM d, yyyy";    
    public const string DateTimeLongFormat = "HH:mm:ss,  dddd, MMMM d, yyyy";
    public const string DateTimeShortFormat = "HH:mm,  ddd, MMM d, yyyy";
    public const string TimeLongFormat = "HH:mm:ss";
    public const string TimeShortFormat = "HH:mm";
}