using DrugSchedule.Services.Models;

namespace DrugSchedule.Services.Services;

public interface ITimetableBuilder
{
    public Task<List<TimetableEntry>> GetScheduleTimetableAsync(List<long> schedulesIds, DateOnly minDate, DateOnly maxDate, bool withConfirmations, CancellationToken cancellationToken = default);
}