using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Constants;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Utils;
using Microsoft.AspNetCore.Components;

namespace DrugSchedule.Client.ViewModels;

public class SchedulesViewModel : PageViewModelBase
{
    [SupplyParameterFromQuery(Name = "id")]
    public long ScheduleIdParameter { get; set; }

    [SupplyParameterFromQuery(Name = "new")]
    public bool NewScheduleParameter { get; set; }


    protected ScheduleExtendedDto? Schedule { get; private set; }
    protected List<TimetableEntryDto>? UpcomingTimetable { get; private set; }

    protected string? MedicamentName => Schedule?.UserMedicament?.Name ?? Schedule?.GlobalMedicament?.Name;
 

    protected override async Task LoadAsync()
    {
        UpcomingTimetable = null;
        if (NewScheduleParameter)
        {
            Schedule = null;
            PageState = PageState.New;
            return;
        } 

        if (ScheduleIdParameter == default)
        {
            Schedule = null;
            PageState = PageState.Default;
            return;
        }

        var result = await ApiClient.GetScheduleExtendedAsync(new ScheduleIdDto { ScheduleId = ScheduleIdParameter });
        if (!result.IsOk)
        {
            await ServeApiCallResult(result);
            ToSchedulesHome();
            return;
        }

        Schedule = result.ResponseDto;

        var timetableFilter = new TimetableFilterDto
        {
            ScheduleId = Schedule.Id,
            MinDate = DateOnly.FromDateTime(DateTime.Now),
            MaxDate = DateOnly.FromDateTime(DateTime.Now.AddDays(20))
        };
        var timetableResult = await ApiClient.GetTimetableAsync(timetableFilter);
        UpcomingTimetable = timetableResult.IsOk ? timetableResult.ResponseDto.TimetableEntries : new();
        PageState = PageState.Details;
    }

    protected void AfterDelete(long id)
    {
        ToSchedulesHome();
    }

    protected void AfterSave(long id)
    {
        NavigationManager.NavigateWithParameter(Routes.Schedules, "id", id);
    }

    protected void ToSchedulesHome()
    {
        NavigationManager.NavigateTo(Routes.Schedules);
    }

    protected void CreateNew()
    {
        NavigationManager.NavigateWithBoolParameter(Routes.Schedules, "new");
    }

    protected void Edit()
    {
        PageState = PageState.Editor;
        StateHasChanged();
    }
}