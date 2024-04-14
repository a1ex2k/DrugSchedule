using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components;
using DrugSchedule.Client.Models;

namespace DrugSchedule.Client.Components;

public partial class Timetable
{
    [Inject] public IApiClient ApiClient { get; set; } = default!;

    private List<DayTimetableModel> Days { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        await LoadTimetableAsync();
        await base.OnInitializedAsync();
    }
    

    private async Task LoadTimetableAsync()
    {
        var filter = new TimetableFilterDto
        {
            MinDate = DateOnly.FromDateTime(DateTime.Now),
            MaxDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            ScheduleId = null
        };

        var result = await ApiClient.GetTimetableAsync(filter);
        if (result.IsOk)
        {
            Days = result.ResponseDto.TimetableEntries
                .GroupBy(x => x.Date)
                .Select(g => new DayTimetableModel
                {
                    Date = g.Key,
                    Entries = g.ToList()
                })
                .OrderBy(x => x.Date)
                .ToList();
        }
        else
        {
            Days = new();
        }
    }
}