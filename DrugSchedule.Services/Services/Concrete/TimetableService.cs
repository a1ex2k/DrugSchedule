using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Services;

public class TimetableBuilder : ITimetableBuilder
{
    private readonly IScheduleRepeatRepository _repeatRepository;
    private readonly IScheduleConfirmationRepository _confirmationRepository;

    public TimetableBuilder(IScheduleRepeatRepository repeatRepository, IScheduleConfirmationRepository confirmationRepository)
    {
        _repeatRepository = repeatRepository;
        _confirmationRepository = confirmationRepository;
    }

    public async Task<List<TimetableEntry>> GetScheduleTimetableAsync(List<long> schedulesIds, DateOnly minDate, DateOnly maxDate, bool withConfirmations, CancellationToken cancellationToken = default)
    {
        var repeats = await _repeatRepository.GetRepeatsAsync(schedulesIds, cancellationToken);

        if (repeats.Count == 0) return new List<TimetableEntry>();

        var timetable = BuildTimetable(repeats, minDate, maxDate);
        if (withConfirmations && timetable.Count > 0)
        {
            await PopulateWithConfirmationIdsAsync(timetable, cancellationToken);
        }

        return timetable;
    }

    private async Task PopulateWithConfirmationIdsAsync(List<TimetableEntry> entries, CancellationToken cancellationToken)
    {
        var repeatIds = entries.ConvertAll(x => x.RepeatId);
        var confirmations =
            await _confirmationRepository.GetTakingConfirmationsForTimetableAsync(repeatIds, cancellationToken);

        foreach (var timetableEntry in entries)
        {
            var confirmation = confirmations.Find(c =>
                c.ForDate == timetableEntry.Date && c.ForTime == timetableEntry.Time &&
                c.ForTimeOfDay == timetableEntry.TimeOfDay);

            timetableEntry.ConfirmationId = confirmation?.Id;
        }
    }

    private List<TimetableEntry> BuildTimetable(List<ScheduleRepeatPlain> repeats, DateOnly minDate, DateOnly maxDate, CancellationToken cancellationToken = default)
    {
        var timeTable = new List<TimetableEntry>();

        foreach (var repeat in repeats)
        {
            var realStartDate = DateUtility.Max(minDate, repeat.BeginDate);
            var realEndDate = DateUtility.Min(maxDate, repeat.EndDate ?? maxDate);
            var dateRange = DateUtility.GetRange(realStartDate, realEndDate, repeat.RepeatDayOfWeek);

            foreach (var day in dateRange)
            {
                var timeTableEntry = new TimetableEntry
                {
                    Date = day,
                    TimeOfDay = repeat.TimeOfDay,
                    Time = repeat.Time,
                    ScheduleId = repeat.MedicamentTakingScheduleId,
                    RepeatId = repeat.Id,
                };
                timeTable.Add(timeTableEntry);
            }
        }

        timeTable.Sort(_comparison);
        return timeTable;
    }


    private readonly Comparison<TimetableEntry> _comparison = delegate(TimetableEntry first, TimetableEntry second)
    {
        var dateCompare = first.Date.CompareTo(second.Date);
        if (dateCompare != 0) return dateCompare;
        var timeOfDayCompare = first.TimeOfDay.CompareTo(second.TimeOfDay);
        if (timeOfDayCompare != 0) return timeOfDayCompare;
        return first.Time.CompareTo(second.Time);
    };
}