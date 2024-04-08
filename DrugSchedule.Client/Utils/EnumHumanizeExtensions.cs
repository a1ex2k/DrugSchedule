using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Utils;

public static class EnumHumanizeExtensions
{
    public static string ToHumanReadableString(this TimeOfDayDto timeOfDay)
    {
        return timeOfDay switch
        {
            TimeOfDayDto.None => "User defined time",
            TimeOfDayDto.MorningWakeup => "At morning wakeup",
            TimeOfDayDto.BeforeBreakfast => "Before breakfast",
            TimeOfDayDto.DuringBreakFast => "During breakfast",
            TimeOfDayDto.AfterBreakfast => "After breakfast",
            TimeOfDayDto.BeforeBrunch => "Before brunch",
            TimeOfDayDto.DuringBrunch => "During brunch",
            TimeOfDayDto.AfterBrunch => "After brunch",
            TimeOfDayDto.BeforeLunch => "Before lunch",
            TimeOfDayDto.DuringLunch => "During lunch",
            TimeOfDayDto.AfterLunch => "After lunch",
            TimeOfDayDto.BeforeNap => "Before nap",
            TimeOfDayDto.AfterNap => "After nap",
            TimeOfDayDto.BeforeDunch => "Before dunch",
            TimeOfDayDto.DuringDunch => "During dunch",
            TimeOfDayDto.AfterDunch => "After dunch",
            TimeOfDayDto.BeforeDinner => "Before dinner",
            TimeOfDayDto.DuringDinner => "During dinner",
            TimeOfDayDto.AfterDinner => "After dinner",
            TimeOfDayDto.BeforeSleep => "Before sleep",
            _ => throw new ArgumentException("Invalid TimeOfDayDto value")
        };
    }

    public static EnumElement<TimeOfDayDto>[] TimeOfDayToArray()
    {
        return Enum.GetValues<TimeOfDayDto>()
            .Select(x => new EnumElement<TimeOfDayDto>(x.ToHumanReadableString(), x))
            .ToArray();
    }

    public static FlagEnumElement<RepeatDayOfWeekDto>[] ToArray(this RepeatDayOfWeekDto enumValue)
    {
        var array = new FlagEnumElement<RepeatDayOfWeekDto>[7];

        array[0] = new ("Sun", RepeatDayOfWeekDto.Sunday, enumValue.HasFlag(RepeatDayOfWeekDto.Sunday));
        array[1] = new ("Mon", RepeatDayOfWeekDto.Monday, enumValue.HasFlag(RepeatDayOfWeekDto.Monday));
        array[2] = new ("Tue", RepeatDayOfWeekDto.Tuesday, enumValue.HasFlag(RepeatDayOfWeekDto.Tuesday));
        array[3] = new ("Wed", RepeatDayOfWeekDto.Wednesday, enumValue.HasFlag(RepeatDayOfWeekDto.Wednesday));
        array[4] = new ("Thu", RepeatDayOfWeekDto.Thursday, enumValue.HasFlag(RepeatDayOfWeekDto.Thursday));
        array[5] = new ("Fri", RepeatDayOfWeekDto.Friday, enumValue.HasFlag(RepeatDayOfWeekDto.Friday));
        array[6] = new ("Sat", RepeatDayOfWeekDto.Saturday, enumValue.HasFlag(RepeatDayOfWeekDto.Saturday));
       
        return array;
    }

    public static RepeatDayOfWeekDto ToEnum(this IEnumerable<FlagEnumElement<RepeatDayOfWeekDto>> enumFlagArray)
    {
        var daysEnum = (RepeatDayOfWeekDto)0;

        foreach (var item in enumFlagArray)
        {
            if (item.Checked)
            {
                daysEnum |= item.Value ;
            }
        }

        return daysEnum;
    }
}