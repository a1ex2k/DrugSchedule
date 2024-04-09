using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Models;

namespace DrugSchedule.Client.Utils;

public static class Converters
{
    public static RepeatModel ToModel(this ScheduleRepeatDto dto)
    {
        return new RepeatModel
        {
            ScheduleId = dto.MedicamentTakingScheduleId,
            RepeatId = dto.Id,
            BeginDate = dto.BeginDate,
            Time = dto.Time?.ToTimeSpan() ?? TimeSpan.FromHours(12),
            TimeOfDay = dto.TimeOfDay,
            EndDate = dto.EndDate,
            TakingRule = dto.TakingRule,
            Days = dto.RepeatDayOfWeek.ToArray()
        };
    }

    public static ScheduleModel ToModel(this ScheduleExtendedDto dto)
    {
        var repeats = dto.ScheduleRepeats
            .Select(x => new KeyValuePair<long, ScheduleRepeatDto>(x.Id, x))
            .ToList();

        return new ScheduleModel
        {
            ScheduleId = dto.Id,
            GlobalMedicament = dto.GlobalMedicament,
            UserMedicament = dto.UserMedicament,
            Information = dto.Information,
            Enabled = dto.Enabled,
            Repeats = repeats,
        };
    }
}